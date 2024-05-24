using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.Chat;
using TeamWork.Core.Models.Fatawa;

namespace TeamWork.Hubs
{
    public static class HubConnections
    {
        public static Dictionary<string, List<string>> ChatUsers = new Dictionary<string, List<string>>();

        public static List<string> GetChatUserId(string name)
        {
            return ChatUsers[name];
        }
    }

    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }


        public async Task SendNewMessageRefresh(QuestionDiscussionModel questionDiscussionModel)
        {
            await Clients.User(questionDiscussionModel.RepliedId.ToString()).SendAsync("SendNewMessageRefresh", questionDiscussionModel);
        }

        public async Task UnreadChattingMessages(int repliedId)
        {
            await Clients.User(repliedId.ToString()).SendAsync("UnreadChattingMessages");
        }

        public async Task AddedNewQuestionRefresh()
        {
            await Clients.Groups("Admin").SendAsync("AddedNewQuestionRefresh");
        }

        public async Task AddedNewFatwaRefresh()
        {
            await Clients.Groups("SuperAdmin").SendAsync("AddedNewFatwaRefresh");
        }

        public async Task PublishedNewFatwaRefresh(int userId)
        {
            await Clients.User(userId.ToString()).SendAsync("PublishedNewFatwaRefresh");
        }

        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated
                && HubConnections.ChatUsers.ContainsKey(Context.User.Identity.Name)
                && !HubConnections.ChatUsers[Context.User.Identity.Name].Contains(Context.ConnectionId))
                HubConnections.ChatUsers[Context.User.Identity.Name].Add(Context.ConnectionId);
            else if (!string.IsNullOrEmpty(Context.User.Identity.Name))
                HubConnections.ChatUsers.Add(Context.User.Identity.Name, new List<string> { Context.ConnectionId });

            if (Context.User.IsInRole("SuperAdmin"))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "SuperAdmin");
            }

            if (Context.User.IsInRole("Admin"))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (!string.IsNullOrEmpty(Context.User.Identity.Name))
                HubConnections.ChatUsers.Remove(Context.User.Identity.Name);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNewMessage(ChatMessageModel chatMessageModel)
        {
            await Clients.User(chatMessageModel.RecipientId.ToString()).SendAsync("MessageReceived", chatMessageModel);
            await Clients.User(chatMessageModel.CreatedBy.ToString()).SendAsync("MessageReceived", chatMessageModel);
        }

        public async Task<string> GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
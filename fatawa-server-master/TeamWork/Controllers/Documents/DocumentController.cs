using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using Microsoft.AspNetCore.Http;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Services;
using System.Linq;
using System.Text.RegularExpressions;

namespace TeamWork.Controllers.Documents
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        IConfiguration _config;
        public DocumentsController(IConfiguration config, IUnitOfWorkService unitOfWorkService)
        {
            _config = config;
            _unitOfWorkService = unitOfWorkService;

        }

        [HttpGet]
        public async Task<IActionResult> Get(string filePath)
        {
            byte[] downloadedFile = new byte[] { };
            try
            {
                downloadedFile = await System.IO.File.ReadAllBytesAsync(filePath);
                return Ok(downloadedFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadDocuments()
        {
            List<DocumentModel> uploadedDocuments = new List<DocumentModel>();
            try
            {
                string filePath = _config["StoredFilesPath"];
                System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                file.Directory.Create();

                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        String uploadedFileName = Path.GetRandomFileName();
                        var fullFilePath = Path.Combine(filePath, uploadedFileName);
                        using (var stream = System.IO.File.Create(fullFilePath))
                        {
                            await formFile.CopyToAsync(stream);
                            uploadedDocuments.Add(new DocumentModel() { Key = uploadedFileName, URL = fullFilePath, Name = formFile.FileName });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest(false);
            }
            return Ok(uploadedDocuments);
        }

        [HttpDelete]
        public IActionResult Delete(string fileName)
        {
            try
            {
                string filePath = _config["StoredFilesPath"];
                var fullFilePath = Path.Combine(filePath, fileName);
                System.IO.File.Delete(fullFilePath);
            }
            catch (Exception)
            {
                BadRequest(false);
            }
            return Ok(true);
        }


        [Route("UploadExcel")]
        [HttpPost]
        public async Task<IActionResult> ExcelUpload()
        {

            List<DocumentModel> uploadedDocuments = new List<DocumentModel>();
            //string filePath = _config["StoredFilesPath"];
            // System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            // file.Directory.Create();
            try
            {


                //foreach (var formFile in Request.Form.Files)
                //{
                //    if (formFile.Length > 0)
                //    {




                var filePath = Path.GetTempFileName();
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var inputStream = new FileStream(filePath, FileMode.Create))
                        {
                            // read file to stream
                            await formFile.CopyToAsync(inputStream);
                            // stream to byte array
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                            // get file name
                            IExcelDataReader reader = null;
                            if (formFile.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(inputStream);
                            }
                            else if (formFile.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(inputStream);
                            }
                            string fName = formFile.FileName;

                            DataSet excelRecords = reader.AsDataSet();
                            reader.Close();
                            var finalRecords = excelRecords.Tables[0];
                            for (int i = 0; i < finalRecords.Rows.Count; i++)
                            {
                                FatawaModel objfatawa = new FatawaModel();
                                objfatawa.Id = 0;
                                objfatawa.Name = finalRecords.Rows[i][0].ToString();
                                objfatawa.FatawaQuestion = finalRecords.Rows[i][1].ToString();
                                objfatawa.Description = finalRecords.Rows[i][2].ToString();
                                objfatawa.FatawaTypeId = 3;
                                objfatawa.FatawaMathhabId = 5;
                                objfatawa.LanguageId = 1;
                                objfatawa.MuftiId = 1;
                                objfatawa.StatusId = 2;
                                objfatawa.FatawaDepartmentId = 1;
                                string tags = finalRecords.Rows[i][0].ToString();
                                // Split authors separated by a comma followed by space  



                                Regex re = new Regex(@"\b\w{1,2}\b");
                                var result = re.Replace(tags, "");

                                result = result.Replace("التي", "");
                                result = result.Replace("على", "");
                                result = result.Replace("الى", "");

                                string[] tagsList = result.Split(" ");
                                List<TagModel> tagModel = new List<TagModel>(); ;

                                for (int k = 0; k < tagsList.Count(); k++)
                                {
                                    string tag = "";

                                    if (k == (tagsList.Count() - 1))
                                    {
                                        tag = tagsList[k];

                                        tagModel.Add(new TagModel() { Name = tag });
                                    }
                                    else
                                    {
                                        tag = string.Concat(string.Concat(tagsList[k], " ", tagsList[k + 1]));

                                        tagModel.Add(new TagModel() { Name = tag });
                                    }
                                }
                           
                                if (tagModel.Count > 0)
                                {
                                    objfatawa.Tags = tagModel.ToList();
                                }
                                else
                                {
                                    tagModel.Add(new TagModel() { Name = " " });
                                    objfatawa.Tags = tagModel.ToList();
                                }   
                                _unitOfWorkService.FatawaService.AddFatawa(objfatawa, CurrentUserId);
                                // objEntity.UserDetails.Add(objUser);

                            }



                        }
                    }
                }
                //String uploadedFileName = formFile.FileName;
                //var fullFilePath = Path.Combine(filePath, uploadedFileName);
                //
                //String uploadedFileName = formFile.FileName;
                //var fullFilePath = Path.Combine(filePath, uploadedFileName);
                //using (var stream = System.IO.File.Create(fullFilePath))
                //{
                //    await formFile.CopyToAsync(stream);
                //    uploadedDocuments.Add(new DocumentModel() { Key = uploadedFileName, URL = fullFilePath, Name = formFile.FileName });

                //}
                //FileStream fileStream = new FileStream(formFile.open, FileMode.Open, FileAccess.Read,);

                //if (formFile.FileName.EndsWith(".xls"))
                //{
                //    reader = ExcelReaderFactory.CreateBinaryReader(fileStream);
                //}
                //else if (formFile.FileName.EndsWith(".xlsx"))
                //{
                //    reader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                //}


                //DataSet excelRecords = reader.AsDataSet();

                //reader.Close();
                //var finalRecords = excelRecords.Tables[0];


                //using (var stream = System.IO.File.Create(fullFilePath))
                //{  //IExcelDataReader reader = null;



                //    //for (int i = 0; i < finalRecords.Rows.Count; i++)
                //    //{
                //    //    FatawaModel model  = new FatawaModel();
                //    //    objUser.UserName = finalRecords.Rows[i][0].ToString();
                //    //    objUser.EmailId = finalRecords.Rows[i][1].ToString();
                //    //    objUser.Gender = finalRecords.Rows[i][2].ToString();
                //    //    objUser.Address = finalRecords.Rows[i][3].ToString();
                //    //    objUser.MobileNo = finalRecords.Rows[i][4].ToString();
                //    //    objUser.PinCode = finalRecords.Rows[i][5].ToString();

                //    //    objEntity.UserDetails.Add(objUser);

                //    //}

                //    //  await formFile.CopyToAsync(stream);
                //    // uploadedDocuments.Add(new DocumentModel() { Key = uploadedFileName, URL = fullFilePath, Name = formFile.FileName });
                //}
                //    }
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
            return Ok(uploadedDocuments);
            //string message = "";
            //HttpResponseMessage result = null;
            // List<DocumentModel> uploadedDocuments = new List<DocumentModel>();
            //try
            //{
            //    string filePath = _config["StoredFilesPath"];
            //    System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            //    file.Directory.Create();

            //    if (httpRequest.Files.Count > 0)
            //    {
            //        HttpPostedFile file = httpRequest.Files[0];
            //        Stream stream = file.InputStream;

            //        IExcelDataReader reader = null;

            //        if (file.FileName.EndsWith(".xls"))
            //        {
            //            reader = ExcelReaderFactory.CreateBinaryReader(stream);
            //        }
            //        else if (file.FileName.EndsWith(".xlsx"))
            //        {
            //            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //        }
            //        else
            //        {
            //            message = "This file format is not supported";
            //        }

            //        DataSet excelRecords = reader.AsDataSet();
            //        reader.Close();

            //        var finalRecords = excelRecords.Tables[0];
            //        for (int i = 0; i < finalRecords.Rows.Count; i++)
            //        {
            //            ////UserDetail objUser = new UserDetail();
            //            //objUser.UserName = finalRecords.Rows[i][0].ToString();
            //            //objUser.EmailId = finalRecords.Rows[i][1].ToString();
            //            //objUser.Gender = finalRecords.Rows[i][2].ToString();
            //            //objUser.Address = finalRecords.Rows[i][3].ToString();
            //            //objUser.MobileNo = finalRecords.Rows[i][4].ToString();
            //            //objUser.PinCode = finalRecords.Rows[i][5].ToString();

            //            //objEntity.UserDetails.Add(objUser);

            //        }

            //        int output = objEntity.SaveChanges();
            //        if (output > 0)
            //        {
            //            message = "Excel file has been successfully uploaded";
            //        }
            //        else
            //        {
            //            message = "Excel file uploaded has fiald";
            //        }

            //    }

            //    else
            //    {
            //        result = Request.CreateResponse(HttpStatusCode.BadRequest);
            //    }
            //}
            //return message;
        }
    }
}

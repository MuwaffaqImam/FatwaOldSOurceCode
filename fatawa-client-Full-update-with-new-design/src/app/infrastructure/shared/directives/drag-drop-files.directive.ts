import {
    Directive,
    EventEmitter,
    HostBinding,
    HostListener,
    Output,
} from '@angular/core';

@Directive({
    selector: '[appDragDropFiles]',
})
export class DragDropFilesDirective {
    @HostBinding('class.is-dragging') isDragging: boolean;
    @Output() emitDropFileList = new EventEmitter<FileList>();

    // Dragover listener
    @HostListener('dragover', ['$event']) onDragOver(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging = true;
    }

    // Dragleave listener
    @HostListener('dragleave', ['$event']) public onDragLeave(
        event: DragEvent,
    ): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging = false;
    }

    // Drop listener
    @HostListener('drop', ['$event']) public onDrop(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging = false;
        const files = event.dataTransfer.files;
        if (files.length > 0) {
            this.emitDropFileList.emit(files);
        }
    }
}

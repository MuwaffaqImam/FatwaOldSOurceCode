import {
    AccordionAnchorDirective,
    AccordionDirective,
    AccordionLinkDirective,
} from './accordion';
import { DynamicFieldDirective } from './dynamic-field.directive';
import { MatTableResponsiveDirective } from './mat-table-responsive/mat-table-responsive.directive';
import { DragDropFilesDirective } from './drag-drop-files.directive';
/**
 * Add directives that do not need to be specifically referenced.
 */
export const directives = [
    DynamicFieldDirective,
    MatTableResponsiveDirective,
    AccordionAnchorDirective,
    AccordionLinkDirective,
    AccordionDirective,
    DragDropFilesDirective,
];

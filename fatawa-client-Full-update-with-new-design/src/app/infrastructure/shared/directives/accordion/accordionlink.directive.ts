import {
    Directive,
    HostBinding,
    Inject,
    Input,
    OnInit,
    OnDestroy,
} from '@angular/core';

import { AccordionDirective } from './accordion.directive';

@Directive({
    selector: '[appAccordionLink]',
})
export class AccordionLinkDirective implements OnInit, OnDestroy {
    @Input() public group: any;

    @HostBinding('class.selected')
    @Input()
    get selected(): boolean {
        return this.menuSelected;
    }

    set selected(value: boolean) {
        this.menuSelected = value;
        if (value) {
            this.nav.closeOtherLinks(this);
        }
    }

    protected menuSelected: boolean;
    protected nav: AccordionDirective;

    constructor(@Inject(AccordionDirective) nav: AccordionDirective) {
        this.nav = nav;
    }

    ngOnInit(): any {
        this.nav.addLink(this);
    }

    ngOnDestroy(): any {
        this.nav.removeGroup(this);
    }

    toggle(): any {
        this.selected = !this.selected;
    }
}

import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-shared-slider',
    templateUrl: './shared-slider.component.html',
    styleUrls: ['./shared-slider.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SharedSliderComponent implements OnInit {
    constructor() {}

    ngOnInit(): void {}
}

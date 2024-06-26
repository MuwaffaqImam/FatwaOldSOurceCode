import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { TagsListComponent } from './tags-list.component';

describe('TagsListComponent', () => {
    let component: TagsListComponent;
    let fixture: ComponentFixture<TagsListComponent>;

    beforeEach(
        waitForAsync(() => {
            TestBed.configureTestingModule({
                declarations: [TagsListComponent],
            }).compileComponents();
        }),
    );

    beforeEach(() => {
        fixture = TestBed.createComponent(TagsListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

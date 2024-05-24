import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TreeuserComponent } from './treeuser.component';

describe('TreeuserComponent', () => {
    let component: TreeuserComponent;
    let fixture: ComponentFixture<TreeuserComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [TreeuserComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(TreeuserComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

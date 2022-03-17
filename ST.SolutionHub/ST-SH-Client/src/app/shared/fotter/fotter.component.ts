import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-fotter',
  templateUrl: './fotter.component.html',
  styleUrls: ['./fotter.component.scss']
})
export class FotterComponent implements OnInit {
  data = new Date();
  constructor() { }

  ngOnInit() {
  }

}

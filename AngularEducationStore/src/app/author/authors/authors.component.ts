import { Component, OnInit } from '@angular/core';
import { AuthorService } from 'src/app/shared/services/author.service';

@Component({
  selector: 'app-authors',
  templateUrl: './authors.component.html',
  styleUrls: ['./authors.component.scss']
})
export class AuthorsComponent implements OnInit {

  constructor(private authorService: AuthorService) { }

  ngOnInit() {
    this.authorService.requestAuthors().subscribe((data) => console.log(data));
  }

}

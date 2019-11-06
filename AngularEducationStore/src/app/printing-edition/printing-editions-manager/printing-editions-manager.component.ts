import { Component, OnInit } from '@angular/core';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { faHighlighter } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { PrintingEditionType } from 'src/app/shared/enums/printing-edition-type';

@Component({
  selector: 'app-printing-editions-manager',
  templateUrl: './printing-editions-manager.component.html',
  styleUrls: ['./printing-editions-manager.component.scss']
})
export class PrintingEdiotionsManagerComponent implements OnInit {

  editIcon = faHighlighter;
  closeIcon = faTimes;
  createIcon = faPlusCircle;


  displayedColumns: string[] = ['id', 'name', 'description', 'category', 'author', 'price', ' '];
  pageSize = 3;
  searchString: string;
  printingEditionTypes = this.printingEditionParams.printingEditionTypes;

  filterModel = new FilterPrintingEditionModel();
  printingEditionModel = new PrintingEditionModel();

  constructor(private printingEditionService: PrintingEditionService, private printingEditionParams: PrintingEditionsParametrs) {
  }

  ngOnInit() {
    this.printingEditionService.getPrintingEditions(this.filterModel).subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
      console.log(data);
    });

  }
  getType(id: number): any {
    return PrintingEditionType[id];
  }
}

import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import * as EmailValidator from 'email-validator';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'EmailClient';
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  userEmail: string = '';
  isDisabled: boolean = true;
  isValidEmail: boolean = false;

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl,
      isHTML5: true,
      allowedMimeType: ['application/vnd.openxmlformats-officedocument.wordprocessingml.document'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }

    this.uploader.onBuildItemForm = (item, form) => {
      form.append('userEmail', this.userEmail); // Use the email address from the form input
    };
  }

  emailValidation(event:any) {
    const email = event.target.value;
    this.isValidEmail = EmailValidator.validate(email);
  }

  onUserInput(event:any){
    // Get the input text
    const inputText = event.target.value;
    if(inputText=='' || !this.isValidEmail){
      this.isDisabled = true;  // Make button disabled
    }
    if (this.isValidEmail){
      this.isDisabled = false; // Make button enabled
    }
  }
}

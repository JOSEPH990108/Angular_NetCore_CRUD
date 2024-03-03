import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Hobby } from '../_models/hobby';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Skillset } from '../_models/skillset';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private usersUpdatedSubject = new Subject<void>();
  paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>;
  
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  getAllFreelancer(page?: number, itemsPerPage?: number){
    let params = new HttpParams();

    if(page && itemsPerPage){
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.get<User[]>(this.baseUrl + 'user', {observe: 'response', params}).pipe(
      map(response => {

        if(response.body){
          this.paginatedResult.result = response.body;
        }

        const pagination = response.headers.get('Pagination');

        if(pagination){
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult
      })
    );
  }

  getFreelancerById(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}user/${id}`).pipe(
      map(user => ({
        id: user.id,
        username: user.username,
        mail: user.mail,
        phoneNumber: user.phoneNumber,
        userHobbies: user.userHobbies,
        userSkillsets: user.userSkillsets
      })),
      catchError(error => {
        this.toastr.error(error.message || 'An error occurred while fetching freelancer details.');
        return throwError(error);
      })
    );
  }

  getAvailableHobbies(): Observable<Hobby[]> {
    return this.http.get<Hobby[]>(this.baseUrl + 'user/available-hobbies').pipe(
      map(hobbies => {
        return hobbies.map(hobby => ({
          id: hobby.id,
          title: hobby.title,
        }));
      }),
      catchError(error => {
        this.toastr.error(error.message || 'An error occurred while fetching available hobbies.');
        return throwError(error);
      })
    );
  }

  getAvailableSkillsets(): Observable<Hobby[]> {
    return this.http.get<Skillset[]>(this.baseUrl + 'user/available-skillsets').pipe(
      map(skillsets => {
        return skillsets.map(skillset => ({
          id: skillset.id,
          title: skillset.title,
        }));
      }),
      catchError(error => {
        this.toastr.error(error.message || 'An error occurred while fetching available skillsets.');
        return throwError(error);
      })
    );
  }

  createUser(userData: User): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}user/register`, userData).pipe(
        catchError(error => {
          this.toastr.error('An error occurred while creating the user.');
          return throwError(error);
        })  
    );
  }

  updateUser(userId: number, userData: User): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}user/update/${userId}`,  userData).pipe(
      catchError(error => {
        this.toastr.error('An error occurred while updating the user.');
        return throwError(error);
      })  
    );
  }

  deleteUser(userId: number) {
    return this.http.delete(`${this.baseUrl}user/delete/${userId}`).pipe(
      catchError(error => {
        this.toastr.error('An error occurred while deleting the user.');
        return throwError(error);
      })  
  );;
  }

  // Method to notify subscribers when a new user is created
  notifyUsersUpdated() {
    this.usersUpdatedSubject.next();
  }

  // Observable to subscribe for user creation events
  onUserUpdated(): Observable<void> {
    return this.usersUpdatedSubject.asObservable();
  }

}


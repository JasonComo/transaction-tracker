import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransactionDTO } from '../shared/transaction-dto.model';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private url = environment.apiBaseUrl + '/Transaction';

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  private createHeaders(): HttpHeaders {
    return this.tokenService.createHeaders();
  }

  getTransactionsByUserId(): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get(`${this.url}/user`, { headers });
  }

  getSpendingBreakdownByPercent(): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get(`${this.url}/spending-breakdown-percent`, { headers });
  }

  getSumofTransactions(): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get(`${this.url}/transaction-sum`, { headers });
  }

  getMostExpensiveTransaction(): Observable<any> {
    const headers = this.createHeaders();
    return this.http.get(`${this.url}/most-expensive`, { headers });
  }

  createTransaction(transactionDTO: TransactionDTO): Observable<any> {
    const headers = this.createHeaders();
    return this.http.post<TransactionDTO>(`${this.url}/create`, transactionDTO, { headers });
  }
  updateTransaction(transactionId: number, transactionDTO: TransactionDTO): Observable<any> {
    const headers = this.createHeaders();
    return this.http.put<void>(`${this.url}/update/${transactionId}`, transactionDTO, { headers });
  }

  deleteTransaction(transactionId: number): Observable<any> {
    const headers = this.createHeaders();
    return this.http.request('delete', `${this.url}/delete`, { headers, body: transactionId })
  }


}

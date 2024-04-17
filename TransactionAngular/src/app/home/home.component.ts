import { Component, OnInit } from '@angular/core';
import { TransactionDTO } from '../shared/transaction-dto.model';
import { TransactionService } from '../services/transaction.service';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  transactions: TransactionDTO[] = [];
  spendingBreakdownByPercent: { [key: string]: number } = {};
  transactionSum: any;
  mostExpensiveTransaction: TransactionDTO = new TransactionDTO();
  newTransaction: TransactionDTO = new TransactionDTO();
  username: string = '';
  updating: boolean = false;


  constructor(private transactionService: TransactionService, private authService: AuthenticationService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.clearForm()
    this.getGetTransactionsByUserId();
    this.getSpendingBreakdownByPercent();
    this.getMostExpensiveTransaction();
    this.getSumOfTransactions();
    this.getUserNameById();
  }

  getGetTransactionsByUserId() {
    this.transactionService.getTransactionsByUserId().subscribe({
      next: (response: any) => {
        this.transactions = response as TransactionDTO[];
        this.transactions.sort((a, b) => new Date(b.dateTime).getTime() - new Date(a.dateTime).getTime());
      },
      error: (error) => {
        console.error('Error fetching user transactions:', error);
      }
    });
  }

  getSpendingBreakdownByPercent() {
    this.transactionService.getSpendingBreakdownByPercent().subscribe({
      next: spendingBreakdownByPercent => {
        this.spendingBreakdownByPercent = spendingBreakdownByPercent;
        console.log(spendingBreakdownByPercent)

        // Modify FoodDrink to be Food & Drink
        if ('FoodDrink' in this.spendingBreakdownByPercent) {
          this.spendingBreakdownByPercent['Food & Drink'] = this.spendingBreakdownByPercent['FoodDrink'];
          delete this.spendingBreakdownByPercent['FoodDrink'];
        }

      },
      error: error => {
        console.error('Error fetching spending breakdown:', error);
      }
    });
  }

  // Present enum categories properly in the dropdown
  getCategoryName(categoryValue: number): string {
    switch (categoryValue) {
      case 0: return 'Food & Drink';
      case 1: return 'Rent';
      case 2: return 'Utilities';
      case 3: return 'Entertainment';
      case 4: return 'Travel';
      case 5: return 'Transportation';
      case 6: return 'Medical';
      case 7: return 'Groceries';
      default: return 'Other';
    }
  }

  // Tests dictionary to see if N/A will be displayed
  isEmpty(obj: any): boolean {
    return Object.keys(obj).length === 0;
  }

  getSumOfTransactions() {
    this.transactionService.getSumofTransactions().subscribe({
      next: transactionSum => {
        this.transactionSum = transactionSum;
      },
      error: error => {
        console.error('Error fetching sum:', error);
      }
    });
  }

  getMostExpensiveTransaction() {
    this.transactionService.getMostExpensiveTransaction().subscribe({
      next: mostExpensiveTransaction => {
        this.mostExpensiveTransaction = mostExpensiveTransaction;

      },
      error: (error) => {
        console.error('Error fetching the most expensive transaction:', error);
      }
    });
  }

  createOrUpdateTransaction() {
    if (this.updating) {
      this.updateTransaction();
    } else {
      this.createTransaction();
    }
  }


  selectTransaction(transaction: TransactionDTO) {
    this.newTransaction = { ...transaction };
    this.updating = true;
    console.log('Selected Transaction:', transaction);
  }

  createTransaction() {
    if (this.newTransaction.name.length < 1 || this.newTransaction.name.length > 24) {
      this.toastr.error('Transaction name must be between 1 and 24 characters.');
      return;
    }


    if (isNaN(this.newTransaction.amount) || this.newTransaction.amount <= 0) {
      this.toastr.error('Transaction amount must be a positive number.');
      return;
    }

    this.transactionService.createTransaction(this.newTransaction).subscribe({
      next: (response: any) => {
        console.log('Transaction created successfully:', response);
        this.toastr.success('Transaction Created.');
        this.ngOnInit();
      },
      error: (error) => {
        console.error('Error creating transaction:', error);
      }
    });
  }

  updateTransaction() {
    if (this.newTransaction.name.length < 1 || this.newTransaction.name.length > 24) {
      this.toastr.error('Transaction name must be between 1 and 24 characters.');
      return;
    }

    this.transactionService.updateTransaction(this.newTransaction.id, this.newTransaction).subscribe({
      next: (response: any) => {
        console.log('Transaction updated successfully', response);
        this.toastr.success('Transaction Updated.');
        this.ngOnInit();
      },
      error: (error) => {
        console.error('Error updating transaction:', error);
      }
    });
  }

  deleteTransaction(transactionId: number) {
    this.transactionService.deleteTransaction(transactionId).subscribe({
      next: () => {

        this.toastr.success('Transaction Deleted.');
        this.ngOnInit();
        console.log("init")
      },
      error: (error) => {
        console.error('Error deleting transaction:', error);
      }
    });
  }


  clearForm() {
    this.newTransaction = new TransactionDTO();
    this.updating = false;
  }

  getUserNameById() {
    this.authService.getUserNameById().subscribe({
      next: (username: string) => {
        this.username = username;
      },
      error: (error: any) => {
        console.error('Error fetching username:', error);
      }
    });
  }

  signOut(): void {
    this.authService.signOut();
    this.router.navigate(['/log-in']);
  }

}





















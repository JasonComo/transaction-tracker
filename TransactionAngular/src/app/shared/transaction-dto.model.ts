export enum Category {
    FoodDrink = 0,
    Rent = 1,
    Utilities = 2,
    Entertainment = 3,
    Travel = 4,
    Transportation = 5,
    Medical = 6,
    Groceries = 7
}


// Define other categories as needed



export class TransactionDTO {
    id: number = 0;
    name: string = "";
    dateTime: Date = new Date();
    amount: number = 0;
    category: Category = Category.FoodDrink;
    applicationUserId: string = "";




}

export interface Transaction {
  id: number;
  amount: number;
  description: string;
  customerFullName: string;
  customerMainPhoneNumber: string;
  transactionType: "Credit" | "Debit";
  customerMainEmailAddress: string;
  customerMainAddress: string;
  createdAt: string;
}

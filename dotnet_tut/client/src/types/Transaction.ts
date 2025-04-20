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
export interface CreateTransaction {
  amount: number;
  description: string;
  customerFullName: string;
  customerMainPhoneNumber: string;
  transactionType: "Credit" | "Debit";
  customerMainEmailAddress: string;
  customerMainAddress: string;
}

export type TransactionFilters = {
  transactionType?: "Credit" | "Debit";
  minAmount?: number;
  maxAmount?: number;
  fromDate?: string;
  toDate?: string;
  customerName?: string;
};

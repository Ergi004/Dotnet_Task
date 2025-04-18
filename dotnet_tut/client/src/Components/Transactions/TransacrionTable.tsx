import { useEffect, useState } from "react";
import { Transaction } from "../../types/Transaction";
import { TransactionService } from "../../services/TransactionService";

const transactionFieldsData = [
  "Transaction Id",
  "Amount",
  "Description",
  "Customer Full Name",
  "Customer Main Phone Number",
  "Customer Main Email Address",
  "Customer Main Address",
  "CreatedAt",
];
export const TransactionTable = () => {
  const [transactions, setTransaction] = useState<Transaction[]>([]);

  const getTransactions = async () => {
    const data = await TransactionService.getTransactions();
    setTransaction(data);
  };

  useEffect(() => {
    getTransactions();
  }, []);

  console.log(transactions);
  return (
    <div className="w-full h-full mt-10 ">
      <div className=" w-full grid grid-cols-8 border-b py-2 gap-x-3">
        {transactionFieldsData.map((item, idx) => (
          <div
            key={idx}
            className="col-span-1 flex items-center justify-center font-semibold text-center text-sm px-2 "
          >
            <span>{item}</span>
          </div>
        ))}
      </div>
      {transactions.map((transaction, idx) => (
        <div key={idx} className="grid grid-cols-8">
          <div className="col-span-1 ">{transaction.id}</div>
          <div className="col-span-1 ">{transaction.amount}</div>
          <div className="col-span-1 ">{transaction.description}</div>
          <div className="col-span-1 ">{transaction.customerFullName}</div>
          <div className="col-span-1 ">
            {transaction.customerMainPhoneNumber}
          </div>
          <div className="col-span-1 ">
            {transaction.customerMainEmailAddress}
          </div>
          <div className="col-span-1 ">{transaction.customerMainAddress}</div>
          <div className="col-span-1 ">{transaction.createdAt}</div>
        </div>
      ))}
    </div>
  );
};

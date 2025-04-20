import { useEffect, useState } from "react";
import { Transaction, TransactionFilters } from "../types/Transaction";
import { TransactionService } from "../services/TransactionService";
import { SlOptionsVertical } from "react-icons/sl";
import { TransactionModal } from "./Modal/CreateTransactionModal";
import { EditTransactionModal } from "./Modal/UpdateTransactionModal";

const transactionFieldsData = [
  "Transaction Id",
  "Amount",
  "Description",
  "Customer Full Name",
  "Customer Main Phone Number",
  "Customer Main Email Address",
  "Customer Main Address",
  "CreatedAt",
  "Actions",
];
export const TransactionTable = () => {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [filters, setFilters] = useState<TransactionFilters>({});

  const getTransactions = async () => {
    const data = await TransactionService.getTransactions(filters);
    console.log(data);
    setTransactions(data);
  };

  useEffect(() => {
    getTransactions();
  }, []);

  const handleFilterChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFilters((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleApplyFilters = async () => {
    await getTransactions();
  };

  const handleClearFilters = async () => {
    const data = await TransactionService.getTransactions();
    setTransactions(data);
    setFilters({});
  };

  return (
    <div className="w-full h-full mt-10 flex flex-col">
      <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-6 gap-4 mb-6 p-4 bg-gray-50  ">
        <select
          name="transactionType"
          onChange={handleFilterChange}
          className="p-2 border "
        >
          <option value="">All Types</option>
          <option value="Credit">Credit</option>
          <option value="Debit">Debit</option>
        </select>

        <input
          type="number"
          name="minAmount"
          placeholder="Min Amount"
          onChange={handleFilterChange}
          className="p-2 border "
        />

        <input
          type="number"
          name="maxAmount"
          placeholder="Max Amount"
          onChange={handleFilterChange}
          className="p-2 border "
        />

        <input
          type="text"
          name="customerName"
          placeholder="Customer Name"
          onChange={handleFilterChange}
          className="p-2 border "
        />

        <button
          onClick={handleApplyFilters}
          className="bg-orange-500 font-semibold text-white p-2  hover:bg-orange-600 transition-all duration-150"
        >
          Apply Filters
        </button>
        <button
          onClick={handleClearFilters}
          className="bg-orange-500 font-semibold text-white p-2  hover:bg-orange-600 transition-all duration-150"
        >
          CLear Filters
        </button>
      </div>

      <TransactionModal />

      <div className=" w-full grid grid-cols-9 border-b py-2 gap-x-3">
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
        <div key={idx} className="grid grid-cols-10 gap-3 py-2 text-sm">
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1 ">{transaction.id}</span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1">{transaction.amount}</span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1">{transaction.description}</span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1">{transaction.customerFullName}</span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1">
              {transaction.customerMainPhoneNumber}
            </span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1">
              {transaction.customerMainEmailAddress}
            </span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1 text-ellipsis">
              {transaction.customerMainAddress}
            </span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1">{transaction.transactionType}</span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <span className="line-clamp-1">{transaction.createdAt}</span>
          </div>
          <div className="col-span-1 w-full flex items-center justify-center text-center ">
            <EditTransactionModal transactionId={transaction.id} />
          </div>
        </div>
      ))}
    </div>
  );
};

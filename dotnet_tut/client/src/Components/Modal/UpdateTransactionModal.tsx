import React, { useState, useEffect } from "react";
import { TransactionService } from "../../services/TransactionService";
import { Transaction } from "../../types/Transaction";
import { SlOptionsVertical } from "react-icons/sl";

interface EditTransactionModalProps {
  transactionId: number | null;
}

export const EditTransactionModal = ({
  transactionId,
}: EditTransactionModalProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [formData, setFormData] = useState<Partial<Transaction>>({
    amount: 0,
    description: "",
    customerFullName: "",
    customerMainPhoneNumber: "",
    transactionType: "Credit",
    customerMainEmailAddress: "",
    customerMainAddress: "",
  });

  useEffect(() => {
    const fetchTransaction = async () => {
      if (transactionId) {
        try {
          const data = await TransactionService.getTransaction(transactionId);
          setFormData(data);
        } catch (error) {
          console.error("Error fetching transaction:", error);
        }
      }
    };

    fetchTransaction();
  }, [transactionId]);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "amount" ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!transactionId) return;

    try {
      await TransactionService.updateTransaction(
        transactionId,
        formData as Transaction
      );
    } catch (error) {
      console.error("Error updating transaction:", error);
    }
  };

  if (!transactionId) return null;

  return (
    <>
      <button
        onClick={() => setIsOpen(true)}
        className="p-2 rounded-full hover:bg-gray-200 transition-all duration-200"
      >
        <SlOptionsVertical />
      </button>
      {isOpen && (
        <div
          onClick={() => setIsOpen(false)}
          className="fixed inset-0 bg-black/30 backdrop-blur-sm z-20"
        />
      )}

      {isOpen && (
        <div className="fixed inset-0 bg-black/30 backdrop-blur-sm z-20 flex items-center justify-center">
          <div
            className="w-96 bg-white rounded-md p-6 shadow-lg"
            onClick={(e) => e.stopPropagation()}
          >
            <h2 className="text-xl font-semibold mb-4">Edit Transaction</h2>

            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium mb-1">
                  Transaction Type
                </label>
                <select
                  name="transactionType"
                  value={formData.transactionType}
                  onChange={handleInputChange}
                  className="w-full p-2 border rounded"
                  required
                >
                  <option value="Credit">Credit</option>
                  <option value="Debit">Debit</option>
                </select>
              </div>

              <div>
                <label className="block text-sm font-medium mb-1">Amount</label>
                <input
                  type="number"
                  name="amount"
                  value={formData.amount || ""}
                  onChange={handleInputChange}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium mb-1">
                  Customer Full Name
                </label>
                <input
                  type="text"
                  name="customerFullName"
                  value={formData.customerFullName}
                  onChange={handleInputChange}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium mb-1">
                  Phone Number
                </label>
                <input
                  type="tel"
                  name="customerMainPhoneNumber"
                  value={formData.customerMainPhoneNumber}
                  onChange={handleInputChange}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium mb-1">
                  Email Address
                </label>
                <input
                  type="email"
                  name="customerMainEmailAddress"
                  value={formData.customerMainEmailAddress}
                  onChange={handleInputChange}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium mb-1">
                  Address
                </label>
                <input
                  type="text"
                  name="customerMainAddress"
                  value={formData.customerMainAddress}
                  onChange={handleInputChange}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium mb-1">
                  Description
                </label>
                <input
                  type="text"
                  name="description"
                  value={formData.description}
                  onChange={handleInputChange}
                  className="w-full p-2 border rounded"
                  required
                />
              </div>

              <div className="flex justify-end gap-2 mt-6">
                <button
                  type="button"
                  onClick={() => setIsOpen(false)}
                  className="px-4 py-2 text-gray-600 hover:text-gray-800"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-orange-500 text-white rounded hover:bg-orange-600"
                >
                  Update
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
};

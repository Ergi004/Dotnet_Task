import React, { useState } from "react";
import { Transaction } from "../../types/Transaction";
import { TransactionService } from "../../services/TransactionService";

export const TransactionModal = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [formData, setFormData] = useState<
    Omit<Transaction, "id" | "createdAt">
  >({
    amount: 0,
    description: "",
    customerFullName: "",
    customerMainPhoneNumber: "",
    transactionType: "Credit",
    customerMainEmailAddress: "",
    customerMainAddress: "",
  });

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
    try {
      await TransactionService.createTransaction(formData);
      setIsOpen(false);
      // Reset form after submission
      setFormData({
        amount: 0,
        description: "",
        customerFullName: "",
        customerMainPhoneNumber: "",
        transactionType: "Credit",
        customerMainEmailAddress: "",
        customerMainAddress: "",
      });
    } catch (error) {
      console.error("Error creating transaction:", error);
    }
  };

  return (
    <>
      <button
        onClick={() => setIsOpen(true)}
        className="bg-orange-500 font-semibold text-white text-sm p-2 mb-4 hover:bg-orange-600 transition-all duration-150"
      >
        Add Transaction
      </button>

      {isOpen && (
        <div
          onClick={() => setIsOpen(false)}
          className="fixed inset-0 bg-black/30 backdrop-blur-sm z-20"
        />
      )}

      {isOpen && (
        <div className="fixed inset-0 flex items-center justify-center z-30">
          <div
            className="w-96 bg-white rounded-md p-6 shadow-lg"
            onClick={(e) => e.stopPropagation()}
          >
            <h2 className="text-xl font-semibold mb-4">
              Create New Transaction
            </h2>

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
                  Create
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </>
  );
};

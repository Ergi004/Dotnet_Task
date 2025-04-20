import {
  TransactionFilters,
  Transaction,
  CreateTransaction,
} from "../types/Transaction";
import axios from "./axiosInstanse";

export const TransactionService = {
  async getTransactions(filters?: TransactionFilters) {
    try {
      const params = new URLSearchParams();

      if (filters) {
        Object.entries(filters).forEach(([key, value]) => {
          if (value !== undefined && value !== null && value !== "") {
            params.append(key, value.toString());
          }
        });
      }

      const response = await axios.get("transactions", { params });
      return response.data;
    } catch (error) {
      throw new Error("Error getting transactions!");
    }
  },

  async getTransaction(id: number) {
    try {
      const response = await axios.get(`transactions/${id}`);
      return response.data;
    } catch (error) {
      throw new Error("Error getting transactions!");
    }
  },

  async createTransaction(transaction: CreateTransaction) {
    try {
      const response = await axios.post("transactions", transaction);
      return response.data;
    } catch (error) {
      throw new Error("Error creating transaction!");
    }
  },

  async updateTransaction(id: number, transaction: Transaction) {
    try {
      const response = await axios.put(`transactions/${id}`, transaction);
      return response.data;
    } catch (error) {
      throw new Error("Error updating transaction!");
    }
  },

  async deleteTransaction(id: number) {
    try {
      await axios.delete(`transactions/${id}`);
    } catch (error) {
      throw new Error("Error deleting transaction!");
    }
  },
};

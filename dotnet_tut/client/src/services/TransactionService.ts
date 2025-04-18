import axios from "./axiosInstanse";
export const TransactionService = {
  async getTransactions() {
    try {
      const response = await axios.get("transactions/");
      return response.data;
    } catch {
      throw new Error("Error getting users!");
    }
  },
};

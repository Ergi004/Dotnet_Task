import "./App.css";
import { TransactionTable } from "./Components/Transactions/TransacrionTable";

function App() {
  return (
    <div className="h-full flex flex-col p-6">
      <div className="w-full max-w-[1400px] mx-auto py-6">
        <h1 className="text-2xl font-bold">Transaction Managment Platform</h1>
        <TransactionTable />
      </div>
    </div>
  );
}

export default App;

import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Toaster } from "./ui/elements/sonner";
import { ThemeProvider } from "./ui/elements/theme-provider";
import Rules from "./ui/screens/Rules";
import Home from "./ui/screens/Home";
import { Header } from "./ui/containers/Header"; // Adjust the import path if needed

const App = () => {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <Router>
        <div className="relative flex flex-col w-screen">
          <Header />
          <div className="relative flex flex-col gap-8 grow items-center justify-start">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/rules" element={<Rules />} />
            </Routes>
          </div>
        </div>
        <Toaster position="bottom-right" />
      </Router>
    </ThemeProvider>
  );
};

export default App;

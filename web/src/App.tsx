import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ThemeProvider } from "next-themes";
import { Toaster } from "sonner";
import Home from "./ui/screens/Home";
import "./App.css";

export default () => {
  return (
    <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
        </Routes>
        <Toaster position="bottom-right" />
      </Router>
    </ThemeProvider>
  );
};

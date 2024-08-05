import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Toaster } from "./ui/elements/sonner";
import { ThemeProvider } from "./ui/elements/theme-provider";
import Rules from "./ui/screens/Rules";
import Home from "./ui/screens/Home";
import { Header } from "./ui/containers/Header"; // Adjust the import path if needed
import background from "/assets/bg-desert.png";
import banners from "/assets/banners.png";

const App = () => {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <Router>
        <div className="absolute inset-0 overflow-hidden">
          <div
            className="absolute inset-0 bg-cover bg-center"
            style={{ backgroundImage: `url('${background}')` }}
          />
        </div>
        <div className="relative flex flex-col w-screen">
          <div className="bg-tinyblue">
            <Header />
          </div>
          <img src={banners} alt="banners" className="w-full h-20" />
          <div className="relative flex flex-col grow items-center justify-start">
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

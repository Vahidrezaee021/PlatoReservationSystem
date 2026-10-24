import React from "react";
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { ConfigProvider, Layout, Button } from "antd";
import faIR from "antd/locale/fa_IR";
import { AuthProvider, useAuth } from "./context/AuthContext";
import { AuthPage } from "./pages/AuthPage";
import { HomePage } from "./pages/HomePage"; // اضافه کردن HomePage
import  StudioDetail  from "./pages/StudioDetail";

const { Header, Content, Footer } = Layout;

// کامپوننت نوبار بالا
const AppNavbar = () => {
  const { user, logout, isAuthenticated } = useAuth();

  return (
    <Header
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        background: "#fff",
        borderBottom: "1px solid #f0f0f0",
      }}
    >
      <div style={{ fontWeight: "bold", fontSize: "18px", color: "#1677ff" }}>
        <Link to="/" style={{ color: "#1677ff", textDecoration: "none" }}>
          سامانه اجاره پلاتو
        </Link>
      </div>
      <div>
        {isAuthenticated ? (
          <div style={{ display: "flex", gap: "12px", alignItems: "center" }}>
            <span>
              خوش آمدید، {user?.fullName} (
              {user?.role === "Owner" ? "مالک پلاتو" : "هنرمند"})
            </span>
            <Button danger onClick={logout}>
              خروج
            </Button>
          </div>
        ) : (
          <Link to="/auth">
            <Button type="primary">ورود / ثبت‌نام</Button>
          </Link>
        )}
      </div>
    </Header>
  );
};

export default function App() {
  return (
    <ConfigProvider direction="rtl" locale={faIR}>
      <AuthProvider>
        <BrowserRouter>
          <Layout style={{ minHeight: "100vh", direction: "rtl" }}>
            <AppNavbar />
            <Content
              style={{
                padding: "24px",
                maxWidth: 1200,
                margin: "0 auto",
                width: "100%",
              }}
            >
              <Routes>
                <Route path="/" element={<HomePage />} />{" "}
                {/* اتصال مسیر اصلی به HomePage */}
                <Route path="/auth" element={<AuthPage />} />
                <Route path="/studio/:id" element={<StudioDetail />} />
                <Route path="/booking/:id" element={<StudioDetail />} />

              </Routes>
            </Content>
            <Footer style={{ textAlign: "center", color: "#888" }}>
              سامانه رزرواسیون پلاتو و فضاهای تمرین هنری ©{" "}
              {new Date().getFullYear()}
            </Footer>
          </Layout>
        </BrowserRouter>
      </AuthProvider>
    </ConfigProvider>
  );
}

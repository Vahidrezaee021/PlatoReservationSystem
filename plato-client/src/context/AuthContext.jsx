import React, { createContext, useContext, useState, useEffect } from "react";
import { authService } from "../api/authService";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // خواندن اطلاعات کاربر از localStorage در ابتدای لود
    const savedUser = authService.getCurrentUser();
    const token = localStorage.getItem("token");
    if (savedUser && token) {
      setUser(savedUser);
    }
    setLoading(false);
  }, []);

  const login = async (credentials) => {
    const data = await authService.login(credentials);
    setUser({
      fullName: data.fullName,
      role: data.role,
    });
    return data;
  };

  const logout = () => {
    authService.logout();
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{ user, login, logout, loading, isAuthenticated: !!user }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);

import axiosClient from "./axiosClient";

export const authService = {
  // ثبت نام کاربر
  register: async (userData) => {
    const response = await axiosClient.post("/auth/register", userData);
    return response.data;
  },

  // ورود و ذخیره توکن
  login: async (credentials) => {
    const response = await axiosClient.post("/auth/login", credentials);
    if (response.data.token) {
      localStorage.setItem("token", response.data.token);
      localStorage.setItem(
        "user",
        JSON.stringify({
          fullName: response.data.fullName,
          role: response.data.role,
        }),
      );
    }
    return response.data;
  },

  // خروج از حساب
  logout: () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  },

  // دریافت اطلاعات کاربر ذخیره شده
  getCurrentUser: () => {
    const userStr = localStorage.getItem("user");
    return userStr ? JSON.parse(userStr) : null;
  },
};

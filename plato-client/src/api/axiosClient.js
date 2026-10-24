import axios from "axios";

// آدرس پورت بک‌اند خودت را در صورت نیاز اینجا تغییر بده
const API_BASE_URL = "http://localhost:5051/api";

const axiosClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// اینترسپتور: اضافه کردن خودکار توکن احراز هویت به ریکوئست‌ها
axiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

export default axiosClient;

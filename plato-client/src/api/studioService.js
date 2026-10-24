import axiosClient from "./axiosClient";

export const studioService = {
  // دریافت لیست همه پلاتوهای فعال
  getAll: async () => {
    const response = await axiosClient.get("/studios");
    return response.data;
  },

  // ایجاد پلاتوی جدید توسط مالک
  create: async (studioData) => {
    const response = await axiosClient.post("/studios", studioData);
    return response.data;
  },
};

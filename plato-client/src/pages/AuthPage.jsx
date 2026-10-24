import React, { useState } from "react";
import { Card, Tabs, Form, Input, Button, Select, message } from "antd";
import {
  UserOutlined,
  LockOutlined,
  MailOutlined,
  PhoneOutlined,
} from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { authService } from "../api/authService";

const { Option } = Select;

export const AuthPage = () => {
  const [activeTab, setActiveTab] = useState("login");
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  // هندلر ورود
  const onFinishLogin = async (values) => {
    try {
      setLoading(true);
      await login(values);
      message.success("با موفقیت وارد شدید!");
      navigate("/");
    } catch (error) {
      message.error(error.response?.data?.message || "خطا در ورود به سیستم.");
    } finally {
      setLoading(false);
    }
  };

  // هندلر ثبت‌نام
  const onFinishRegister = async (values) => {
    try {
      setLoading(true);
      await authService.register(values);
      message.success("ثبت‌نام با موفقیت انجام شد. اکنون وارد شوید.");
      setActiveTab("login");
    } catch (error) {
      message.error(error.response?.data?.message || "خطا در ثبت‌نام.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        minHeight: "85vh",
        direction: "rtl",
      }}
    >
      <Card
        style={{
          width: 420,
          boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
          borderRadius: 12,
        }}
      >
        <h2 style={{ textAlign: "center", marginBottom: 24, color: "#1677ff" }}>
          سامانه رزرواسیون پلاتو
        </h2>

        <Tabs
          activeKey={activeTab}
          onChange={setActiveTab}
          centered
          items={[
            {
              key: "login",
              label: "ورود به حساب",
              children: (
                <Form layout="vertical" onFinish={onFinishLogin}>
                  <Form.Item
                    name="email"
                    label="ایمیل"
                    rules={[
                      {
                        required: true,
                        message: "لطفاً ایمیل خود را وارد کنید!",
                      },
                      { type: "email", message: "فرمت ایمیل صحیح نیست!" },
                    ]}
                  >
                    <Input
                      prefix={<MailOutlined />}
                      placeholder="example@mail.com"
                    />
                  </Form.Item>

                  <Form.Item
                    name="password"
                    label="رمز عبور"
                    rules={[
                      {
                        required: true,
                        message: "لطفاً رمز عبور را وارد کنید!",
                      },
                    ]}
                  >
                    <Input.Password
                      prefix={<LockOutlined />}
                      placeholder="رمز عبور"
                    />
                  </Form.Item>

                  <Button
                    type="primary"
                    htmlType="submit"
                    block
                    loading={loading}
                    style={{ marginTop: 12 }}
                  >
                    ورود
                  </Button>
                </Form>
              ),
            },
            {
              key: "register",
              label: "ثبت‌نام جدید",
              children: (
                <Form layout="vertical" onFinish={onFinishRegister}>
                  <Form.Item
                    name="fullName"
                    label="نام و نام خانوادگی"
                    rules={[
                      {
                        required: true,
                        message: "لطفاً نام خود را وارد کنید!",
                      },
                    ]}
                  >
                    <Input prefix={<UserOutlined />} placeholder="علی رضایی" />
                  </Form.Item>

                  <Form.Item
                    name="email"
                    label="ایمیل"
                    rules={[
                      {
                        required: true,
                        message: "لطفاً ایمیل معتبر وارد کنید!",
                      },
                      { type: "email", message: "فرمت ایمیل نامعتبر است!" },
                    ]}
                  >
                    <Input
                      prefix={<MailOutlined />}
                      placeholder="example@mail.com"
                    />
                  </Form.Item>

                  <Form.Item
                    name="phoneNumber"
                    label="شماره تماس"
                    rules={[
                      {
                        required: true,
                        message: "لطفاً شماره تماس را وارد کنید!",
                      },
                    ]}
                  >
                    <Input prefix={<PhoneOutlined />} placeholder="0912..." />
                  </Form.Item>

                  <Form.Item
                    name="password"
                    label="رمز عبور"
                    rules={[
                      {
                        required: true,
                        message: "حداقل ۶ کاراکتر وارد کنید!",
                        min: 6,
                      },
                    ]}
                  >
                    <Input.Password
                      prefix={<LockOutlined />}
                      placeholder="رمز عبور"
                    />
                  </Form.Item>

                  <Form.Item
                    name="role"
                    label="نوع حساب کاربری"
                    initialValue="Artist"
                    rules={[{ required: true }]}
                  >
                    <Select>
                      <Option value="Artist">هنرمند / رزروکننده</Option>
                      <Option value="Owner">صاحب پلاتو / مدیر سالن</Option>
                    </Select>
                  </Form.Item>

                  <Button
                    type="primary"
                    htmlType="submit"
                    block
                    loading={loading}
                    style={{ marginTop: 12 }}
                  >
                    عضویت
                  </Button>
                </Form>
              ),
            },
          ]}
        />
      </Card>
    </div>
  );
};

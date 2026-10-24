import React, { useState } from "react";
import { Modal, Form, Input, InputNumber, Button, Space, message } from "antd";
import { PlusOutlined, MinusCircleOutlined } from "@ant-design/icons";
import { studioService } from "../api/studioService";

export const CreateStudioModal = ({ open, onCancel, onSuccess }) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (values) => {
    try {
      setLoading(true);
      // تبدیل امکانات از آرایه آبجکت یا ساختار فرم به آرایه ساده رشته‌ها
      const formattedData = {
        ...values,
        features: values.features
          ? values.features.map((f) => (typeof f === "object" ? f.title : f))
          : [],
      };

      await studioService.create(formattedData);
      message.success("پلاتو با موفقیت ثبت شد!");
      form.resetFields();
      onSuccess();
    } catch (error) {
      console.error(error);
      message.error(error.response?.data?.message || "خطا در ثبت پلاتو.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal
      title="ثبت پلاتوی جدید"
      open={open}
      onCancel={onCancel}
      footer={null}
      destroyOnClose
    >
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item
          name="title"
          label="عنوان پلاتو"
          rules={[
            { required: true, message: "لطفاً عنوان پلاتو را وارد کنید" },
          ]}
        >
          <Input placeholder="مثال: پلاتو آفتاب - سالن تمرین تئاتر" />
        </Form.Item>

        <Form.Item
          name="address"
          label="آدرس پلاتو"
          rules={[{ required: true, message: "لطفاً آدرس را وارد کنید" }]}
        >
          <Input placeholder="مثال: تهران، خیابان انقلاب، نبش وصال" />
        </Form.Item>

        <div style={{ display: "flex", gap: "16px" }}>
          <Form.Item
            name="areaSquareMeters"
            label="متراژ (متر مربع)"
            rules={[{ required: true, message: "متراژ را وارد کنید" }]}
            style={{ flex: 1 }}
          >
            <InputNumber
              min={10}
              max={1000}
              style={{ width: "100%" }}
              placeholder="مثال: 50"
            />
          </Form.Item>

          <Form.Item
            name="hourlyRate"
            label="نرخ ساعتی (تومان)"
            rules={[{ required: true, message: "نرخ ساعتی را وارد کنید" }]}
            style={{ flex: 1 }}
          >
            <InputNumber
              min={10000}
              step={10000}
              formatter={(value) =>
                `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ",")
              }
              parser={(value) => value.replace(/\$\s?|(,*)/g, "")}
              style={{ width: "100%" }}
              placeholder="مثال: 150,000"
            />
          </Form.Item>
        </div>

        <Form.Item name="description" label="توضیحات و قوانین">
          <Input.TextArea
            rows={3}
            placeholder="توضیحات مربوط به نورپردازی، کف‌پوش و امکانات جانبی..."
          />
        </Form.Item>

        {/* لیست امکانات با Dynamic Form List */}
        <Form.List name="features">
          {(fields, { add, remove }) => (
            <>
              <label style={{ display: "block", marginBottom: 8 }}>
                امکانات و تجهیزات پلاتو:
              </label>
              {fields.map((field) => (
                <Space
                  key={field.key}
                  style={{ display: "flex", marginBottom: 8 }}
                  align="baseline"
                >
                  <Form.Item
                    {...field}
                    name={[field.name, "title"]}
                    rules={[
                      { required: true, message: "نام امکانات را بنویسید" },
                    ]}
                    noStyle
                  >
                    <Input
                      placeholder="مثال: آینه سرتاسری / سیستم صوتی بلوتوث"
                      style={{ width: "280px" }}
                    />
                  </Form.Item>
                  <MinusCircleOutlined
                    onClick={() => remove(field.name)}
                    style={{ color: "#ff4d4f" }}
                  />
                </Space>
              ))}
              <Form.Item>
                <Button
                  type="dashed"
                  onClick={() => add()}
                  block
                  icon={<PlusOutlined />}
                >
                  افزودن امکانات پلاتو
                </Button>
              </Form.Item>
            </>
          )}
        </Form.List>

        <Form.Item style={{ marginBottom: 0, textAlign: "left" }}>
          <Space>
            <Button onClick={onCancel}>انصراف</Button>
            <Button type="primary" htmlType="submit" loading={loading}>
              ثبت و انتشار پلاتو
            </Button>
          </Space>
        </Form.Item>
      </Form>
    </Modal>
  );
};

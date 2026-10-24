import React, { useEffect, useState } from "react";
import { Row, Col, Card, Tag, Button, Spin, Empty, Typography } from "antd";
import {
  EnvironmentOutlined,
  AppstoreOutlined,
  PlusOutlined,
} from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import { studioService } from "../api/studioService";
import { useAuth } from "../context/AuthContext";
import { CreateStudioModal } from "../components/CreateStudioModal";

const { Title, Paragraph } = Typography;

export const HomePage = () => {
  const [studios, setStudios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const { user } = useAuth();
  const navigate = useNavigate();

  const fetchStudios = async () => {
    try {
      setLoading(true);
      const data = await studioService.getAll();
      setStudios(data);
    } catch (error) {
      console.error("خطا در گرفتن پلاتوها:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchStudios();
  }, []);

  return (
    <div>
      {/* سربرگ معرفی و دکمه افزودن پلاتو مخصوص مالکان */}
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          marginBottom: 32,
        }}
      >
        <div>
          <Title level={2} style={{ marginBottom: 8 }}>
            لیست پلاتوها و فضاهای تمرین
          </Title>
          <Paragraph type="secondary">
            پلاتوی مورد نظر خود را برای تمرین تئاتر، موسیقی و ورکشاپ به صورت
            آنلاین رزرو کنید.
          </Paragraph>
        </div>
        {user?.role === "Owner" && (
          <Button
            type="primary"
            icon={<PlusOutlined />}
            size="large"
            onClick={() => setIsModalOpen(true)}
          >
            ثبت پلاتوی جدید
          </Button>
        )}
      </div>

      {/* نمایش لیست یا لودینگ */}
      {loading ? (
        <div style={{ textAlign: "center", padding: "60px 0" }}>
          <Spin size="large" />
        </div>
      ) : studios.length === 0 ? (
        <Empty description="هنوز هیچ پلاتویی ثبت نشده است. اگر مالک هستید، اولین پلاتو را اضافه کنید!" />
      ) : (
        <Row gutter={[24, 24]}>
          {studios.map((studio) => (
            <Col xs={24} sm={12} md={8} key={studio.id}>
              <Card
                hoverable
                style={{
                  height: "100%",
                  display: "flex",
                  flexDirection: "column",
                  borderRadius: 12,
                }}
                bodyStyle={{
                  flex: 1,
                  display: "flex",
                  flexDirection: "column",
                }}
              >
                <Title level={4} style={{ marginBottom: 8 }}>
                  {studio.title}
                </Title>

                <p
                  style={{
                    color: "#666",
                    fontSize: "13px",
                    display: "flex",
                    alignItems: "center",
                    gap: 6,
                  }}
                >
                  <EnvironmentOutlined /> {studio.address}
                </p>

                <p
                  style={{
                    color: "#888",
                    fontSize: "12px",
                    display: "flex",
                    alignItems: "center",
                    gap: 6,
                  }}
                >
                  <AppstoreOutlined /> مساحت: {studio.areaSquareMeters} متر مربع
                </p>

                <Paragraph
                  ellipsis={{ rows: 2 }}
                  style={{ color: "#555", marginTop: 8 }}
                >
                  {studio.description || "توضیحاتی ثبت نشده است."}
                </Paragraph>

                {/* نمایش امکانات پلاتو */}
                <div style={{ marginBottom: 16 }}>
                  {(studio.features || []).map((feat, idx) => (
                    <Tag color="blue" key={idx} style={{ marginBottom: 4 }}>
                      {feat.title}
                    </Tag>
                  ))}
                </div>

                <div
                  style={{
                    marginTop: "auto",
                    paddingTop: 16,
                    borderTop: "1px solid #f0f0f0",
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                  }}
                >
                  <div>
                    <span
                      style={{
                        fontSize: "18px",
                        fontWeight: "bold",
                        color: "#1677ff",
                      }}
                    >
                      {Number(studio.hourlyRate).toLocaleString("fa-IR")}
                    </span>
                    <span style={{ fontSize: "12px", color: "#888" }}>
                      {" "}
                      تومان / ساعت
                    </span>
                  </div>

                  <Button
                    type="primary"
                    onClick={() => navigate(`/booking/${studio.id}`)}
                  >
                    رزرو پلاتو
                  </Button>
                </div>
              </Card>
            </Col>
          ))}
        </Row>
      )}

      {/* مودال افزودن پلاتو */}
      <CreateStudioModal
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onSuccess={() => {
          setIsModalOpen(false);
          fetchStudios();
        }}
      />
    </div>
  );
};

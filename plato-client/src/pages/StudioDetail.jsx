import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { studioService } from '../api/studioService';
import { bookingService } from '../api/bookingService';
import { Card, Button, DatePicker, TimePicker, message, Spin, Tag, Divider, Row, Col, Statistic } from 'antd';
import { CalendarOutlined, EnvironmentOutlined, DollarOutlined, ClockCircleOutlined, ArrowRightOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';

export default function StudioDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [studio, setStudio] = useState(null);
  const [loading, setLoading] = useState(true);
  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedRange, setSelectedRange] = useState([]);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    loadStudio();
  }, [id]);

  const loadStudio = async () => {
    try {
      const res = await studioService.getById(id);
      setStudio(res.data);
    } catch (err) {
      message.error('خطا در بارگذاری اطلاعات پلاتو');
    } finally {
      setLoading(false);
    }
  };

  const handleBooking = async () => {
    if (!selectedDate || selectedRange.length < 2) {
      message.warning('لطفاً تاریخ و بازه ساعت شروع و پایان را به دقت انتخاب کنید.');
      return;
    }

    const dateStr = selectedDate.format('YYYY-MM-DD');
    const startTimeStr = selectedRange[0].format('HH:mm:ss');
    const endTimeStr = selectedRange[1].format('HH:mm:ss');

    const startDateTime = `${dateStr}T${startTimeStr}`;
    const endDateTime = `${dateStr}T${endTimeStr}`;

    try {
      setSubmitting(true);
      await bookingService.createBooking({
        studioId: Number(id),
        startTime: startDateTime,
        endTime: endDateTime
      });
      message.success('رزرو پلاتو با موفقیت انجام شد!');
      navigate('/');
    } catch (err) {
      message.error(err.response?.data?.message || 'خطا در ثبت رزرو. ممکن است این زمان قبلاً رزرو شده باشد.');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-96">
        <Spin size="large" tip="در حال بارگذاری جزئیات پلاتو..." />
      </div>
    );
  }

  if (!studio) {
    return <div className="text-center py-20 text-lg text-red-500">پلاتوی مورد نظر یافت نشد.</div>;
  }

  const hoursCount = selectedRange.length === 2 ? selectedRange[1].diff(selectedRange[0], 'hour', true) : 0;
  const totalPrice = hoursCount > 0 ? Math.round(hoursCount * studio.hourlyRate) : 0;

  return (
    <div className="max-w-6xl mx-auto px-4 py-8">
      <Button icon={<ArrowRightOutlined />} onClick={() => navigate(-1)} className="mb-6">
        بازگشت به لیست پلاتوها
      </Button>

      <Row gutter={[32, 32]}>
        {/* ستون راست: اطلاعات پلاتو */}
        <Col xs={24} lg={14}>
          <div className="rounded-2xl overflow-hidden shadow-lg mb-6 bg-white border border-slate-100">
            <img 
              src={studio.imageUrl || 'https://images.unsplash.com/photo-1516450360452-9312f5e86fc7?auto=format&fit=crop&w=800&q=80'} 
              alt={studio.title} 
              className="w-full h-80 object-cover"
            />
            <div className="p-6">
              <div className="flex justify-between items-start mb-4">
                <h1 className="text-2xl font-bold text-slate-800">{studio.title}</h1>
                <Tag color="purple" className="text-sm px-3 py-1">{studio.city || 'تهران'}</Tag>
              </div>

              <p className="text-slate-600 mb-6 leading-relaxed">{studio.description}</p>

              <div className="grid grid-cols-2 gap-4 mb-6 bg-slate-50 p-4 rounded-xl border border-slate-100">
                <div className="flex items-center gap-2 text-slate-700">
                  <EnvironmentOutlined className="text-indigo-500 text-lg" />
                  <span>{studio.address}</span>
                </div>
                <div className="flex items-center gap-2 text-slate-700">
                  <ClockCircleOutlined className="text-indigo-500 text-lg" />
                  <span>ساعت کاری: {studio.openTime} تا {studio.closeTime}</span>
                </div>
              </div>

              <Divider />

              <h3 className="font-semibold text-slate-800 mb-3">امکانات پلاتو</h3>
              <div className="flex flex-wrap gap-2">
                {studio.features && studio.features.length > 0 ? (
                  studio.features.map((feat, idx) => (
                    <Tag key={idx} color="cyan" className="px-3 py-1 text-sm">{feat.name || feat}</Tag>
                  ))
                ) : (
                  <span className="text-slate-400 text-sm">امکانات خاصی ثبت نشده است.</span>
                )}
              </div>
            </div>
          </div>
        </Col>

        {/* ستون چپ: پنل رزرو */}
        <Col xs={24} lg={10}>
          <Card title="رزرو آنلاین پلاتو" className="shadow-lg rounded-2xl border border-slate-100 sticky top-6">
            <div className="mb-6">
              <span className="text-slate-500 text-sm">نرخ هر ساعت:</span>
              <div className="text-xl font-bold text-indigo-600">
                {studio.hourlyRate?.toLocaleString()} تومان
              </div>
            </div>

            <div className="space-y-4 mb-6">
              <div>
                <label className="block text-slate-700 font-medium mb-2">انتخاب تاریخ رزرو:</label>
                <DatePicker 
                  className="w-full" 
                  size="large" 
                  placeholder="روز رزرو را انتخاب کنید"
                  onChange={(val) => setSelectedDate(val)}
                  disabledDate={(current) => current && current < dayjs().startOf('day')}
                />
              </div>

              <div>
                <label className="block text-slate-700 font-medium mb-2">بازه ساعت (شروع و پایان):</label>
                <TimePicker.RangeView 
                  // یا TimePicker.RangePicker بسته به نسخه antd
                />
                <TimePicker.RangePicker 
                  className="w-full" 
                  size="large"
                  format="HH:mm"
                  minuteStep={30}
                  onChange={(val) => setSelectedRange(val)}
                />
              </div>
            </div>

            {totalPrice > 0 && (
              <div className="bg-indigo-50 p-4 rounded-xl mb-6 border border-indigo-100 flex justify-between items-center">
                <span className="text-slate-700 font-medium">مبلغ قابل پرداخت:</span>
                <span className="text-xl font-extrabold text-indigo-700">{totalPrice.toLocaleString()} تومان</span>
              </div>
            )}

            <Button 
              type="primary" 
              size="large" 
              block 
              loading={submitting}
              onClick={handleBooking}
              className="bg-indigo-600 hover:bg-indigo-700 h-12 font-bold text-base"
            >
              تایید و ثبت نهایی رزرو
            </Button>
          </Card>
        </Col>
      </Row>
    </div>
  );
}

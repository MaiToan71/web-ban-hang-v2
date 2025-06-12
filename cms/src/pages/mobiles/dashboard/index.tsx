import { BsFillFileBarGraphFill } from "react-icons/bs";
import { Col, Row, Flex, Card, Image } from "antd";
// Import Swiper React components
import { Swiper, SwiperSlide } from "swiper/react";

// Import Swiper styles
import "swiper/css";
export default function Dashboard() {
  return (
    <Row>
      <Col span={24}>
        <Flex>
          <BsFillFileBarGraphFill style={{ marginTop: "2px" }} />
          <h1 style={{ fontWeight: "500" }}>Top doanh thu</h1>
        </Flex>
        <Swiper
          style={{ marginLeft: "-200px" }}
          className="mySwiper"
          slidesPerView={"auto"}
          centeredSlides={true}
          spaceBetween={10}
        >
          <SwiperSlide style={{ width: "145px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Doanh thu top 1</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "145px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Doanh thu top 1</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "145px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Doanh thu top 1</p>
            </Card>
          </SwiperSlide>
        </Swiper>
      </Col>

      <Col span={24} className="mt-5">
        <Flex>
          <BsFillFileBarGraphFill style={{ marginTop: "2px" }} />
          <h1 style={{ fontWeight: "500" }}>Khuyến mại</h1>
        </Flex>
        <Swiper
          style={{ marginLeft: "-120px" }}
          className="mySwiper"
          slidesPerView={"auto"}
          centeredSlides={true}
          spaceBetween={10}
        >
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
        </Swiper>
      </Col>

      <Col span={24} className="mt-5">
        <Flex>
          <BsFillFileBarGraphFill style={{ marginTop: "2px" }} />
          <h1 style={{ fontWeight: "500" }}>Hỗ trợ</h1>
        </Flex>
        <Swiper
          style={{ marginLeft: "-200px" }}
          className="mySwiper"
          slidesPerView={"auto"}
          centeredSlides={true}
          spaceBetween={10}
        >
          <SwiperSlide style={{ width: "145px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Doanh thu top 1</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "145px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Doanh thu top 1</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "145px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Doanh thu top 1</p>
            </Card>
          </SwiperSlide>
        </Swiper>
      </Col>

      <Col span={24} className="mt-5">
        <Flex>
          <BsFillFileBarGraphFill style={{ marginTop: "2px" }} />
          <h1 style={{ fontWeight: "500" }}>Đánh giá</h1>
        </Flex>
        <Swiper
          style={{ marginLeft: "-120px" }}
          className="mySwiper"
          slidesPerView={"auto"}
          centeredSlides={true}
          spaceBetween={10}
        >
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Flex>
                <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
                <p>Tên chương trình khuyến mãi</p>
              </Flex>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
        </Swiper>
      </Col>

      <Col span={24} className="mt-5">
        <Flex>
          <BsFillFileBarGraphFill style={{ marginTop: "2px" }} />
          <h1 style={{ fontWeight: "500" }}>Danh sách hàng hóa</h1>
        </Flex>
        <Swiper
          style={{ marginLeft: "-120px" }}
          className="mySwiper"
          slidesPerView={"auto"}
          centeredSlides={true}
          spaceBetween={10}
        >
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
        </Swiper>
      </Col>

      <Col span={24} className="mt-5">
        <Flex>
          <BsFillFileBarGraphFill style={{ marginTop: "2px" }} />
          <h1 style={{ fontWeight: "500" }}>Hạng mục mua nhiều</h1>
        </Flex>
        <Swiper
          style={{ marginLeft: "-120px" }}
          className="mySwiper"
          slidesPerView={"auto"}
          centeredSlides={true}
          spaceBetween={10}
        >
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
        </Swiper>
      </Col>

      <Col span={24} className="mt-5 mb-5">
        <Flex>
          <BsFillFileBarGraphFill style={{ marginTop: "2px" }} />
          <h1 style={{ fontWeight: "500" }}>Khách sạn giảm giá</h1>
        </Flex>
        <Swiper
          style={{ marginLeft: "-120px" }}
          className="mySwiper"
          slidesPerView={"auto"}
          centeredSlides={true}
          spaceBetween={10}
        >
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
          <SwiperSlide style={{ width: "225px" }}>
            <Card>
              <Image src="https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg" />
              <p>Tên chương trình khuyến mãi</p>
              <p style={{ color: "#616161" }}>Nội dung khuyến mãi</p>
            </Card>
          </SwiperSlide>
        </Swiper>
      </Col>
    </Row>
  );
}

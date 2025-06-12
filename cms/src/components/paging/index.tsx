import React from "react";
import { Pagination } from "antd";
// import { useParams, redirect, useNavigate, useResolvedPath  } from 'react-router-dom'

const Paging = (props: any) => {
  const size: any = Number(
    props.size == undefined
      ? import.meta.env.VITE_REACT_APP_API_SIZE
      : props.size
  );

  const handlePagination = (page: any) => {
    props.handlePagination(page);
  };

  return (
    <React.Fragment>
      <div style={{ display: "flex", justifyContent: "end" }}>
        <Pagination
          style={{ marginTop: 16 }}
          defaultCurrent={1}
          current={props.currentPage}
          total={props.total}
          pageSize={size}
          hideOnSinglePage={true}
          onChange={handlePagination}
          showSizeChanger={false}
        />
      </div>
    </React.Fragment>
  );
};
export default Paging;

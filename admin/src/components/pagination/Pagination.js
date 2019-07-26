import React, { Component } from 'react';
import ReactPaginate from 'react-paginate';
import './Pagination.scss'

export default class Pagination extends Component {
  render() {
    let { totalPages, initialPage, forcePage, pageRangeDisplayed, onPageChange } = this.props;
    return (
      totalPages > 0 && <ReactPaginate
        disableInitialCallback={true}
        initialPage={initialPage}
        forcePage={forcePage}
        previousLabel={<i className="fa fa-chevron-left" aria-hidden="true"></i>}
        nextLabel={<i className="fa fa-chevron-right" aria-hidden="true"></i>}
        breakLabel={'...'}
        pageClassName={'page-item'}
        pageLinkClassName="page-link"
        previousClassName='page-item'
        previousLinkClassName='page-link'
        nextClassName='page-item'
        nextLinkClassName='page-link'
        pageCount={totalPages}
        marginPagesDisplayed={2}
        pageRangeDisplayed={pageRangeDisplayed || 1}
        onPageChange={onPageChange}
        containerClassName={'pagination'}
        activeLinkClassName={'active'}
        activeClassName={'active'}
      />
    )
  }
}

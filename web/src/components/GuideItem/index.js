import React from 'react'
import { Container, Row, Col } from 'reactstrap'

const GuideItem = ({ sm, md, index, title, description }) => {
  return (
    <Col className="guide-item" lg={md} md={sm} sm={sm} xs={sm}>
      <Container className="guide-container">
        <Row className="guide-layout guide-top">
          <Col className="step-guide"> Bước {index}</Col>
        </Row>

        <Row className="guide-layout guide-bottom">
          <Col>
            <div className="step-detail"> {title}</div>
            <div className="step-description"> {description}</div>
          </Col>
        </Row>
      </Container>
    </Col>
  )
}

export default GuideItem

import React, { Component } from 'react';
import {
    Navbar,
    Nav,
    NavItem,
    NavLink,
    Container,
    Row,
    Col
} from 'reactstrap';

export class Layout extends Component {
   static displayName = Layout.name;


    render() {
   
    return (
        <div className="logo" >
            <Container className="container-fluid" fluid={true} style={{ padding: "20px", margin: "20px" }}>
                <Row>
                <Col>
                    <img src="/img/apexa_logo.PNG" alt="apexa" />
                </Col>
                <Col> 
                <Navbar variant="light" className="justify-content-end">
                    <Nav>
                        <NavItem>
                            <NavLink href="/">Home</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink href="/advisor">Advisors</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink href="/carrier">Carriers</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink href="/mga">MGAs</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink href="/contract">Contracts</NavLink>
                        </NavItem>
                        </Nav>
                    </Navbar>
                </Col>
                </Row>
            </Container>
            
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}

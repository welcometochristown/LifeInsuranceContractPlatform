import React, { Component } from 'react';
import { Business } from './Business.js'
import { Jumbotron, Container  } from 'reactstrap';

export class Carrier extends Component {
    static displayName = Carrier.name;

    constructor(props) {
        super(props);
        this.state = { carriers: [], loading: true };
    }

    componentDidMount() {
        this.loadCarriers();
    }

    async createCarrier(carrier) {
        await fetch('https://localhost:44363/api/carrier/', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(carrier)
        });
        this.loadCarriers();
    }

    async editCarrier(carrier) {
        await fetch('https://localhost:44363/api/carrier/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(carrier)
        });
        this.loadCarriers();
    }

    async deleteCarrier(carrier) {
        await fetch('https://localhost:44363/api/carrier/' + carrier.id, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });
        this.loadCarriers();
    }

    async loadCarriers() {
        const response = await fetch('https://localhost:44363/api/carrier', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        const data = await response.json();
        this.setState({ carriers: data, loading: false });
    }

    async createContract(entity1, entity2) {
        await fetch('https://localhost:44363/api/contract/establish/' + entity1 + '/' + entity2, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        this.loadCarriers();
    }

    async deleteContract(entity1, entity2) {
        await fetch('https://localhost:44363/api/contract/terminate/' + entity1 + '/' + entity2, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        this.loadCarriers();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <Business
                businesses={this.state.carriers}
                createBusiness={this.createCarrier.bind(this)}
                editBusiness={this.editCarrier.bind(this)}
                deleteBusiness={this.deleteCarrier.bind(this)}
                createContract={this.createContract.bind(this)}
                deleteContract={this.deleteContract.bind(this)}
                type="Carrier"
            />;

        return (
            <div>
                <Jumbotron fluid>
                    <Container fluid>
                        <h1 className="display-3">Carrier</h1>
                        <p className="lead">An insurance company or issuer of insurance products generally in the main business of providing insurance against disability or death, as well as annuities and pensions.</p>
                    </Container>
                </Jumbotron>
                {contents}
            </div>
        );
    }
}

import React, { Component } from 'react';
import { Business } from './Business.js'
import { Jumbotron, Container  } from 'reactstrap';

export class MGA extends Component {
    static displayName = MGA.name;

    constructor(props) {
        super(props);
        this.state = { mgas: [], loading: true };
    }

    componentDidMount() {
        this.loadMGAs();
    }

    async editMGA(mga) {
        await fetch('https://localhost:44363/api/mga', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(mga)
        });
        this.loadMGAs();
    }

    async deleteMGA(mga) {
        await fetch('https://localhost:44363/api/mga/' + mga.id, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });
        this.loadMGAs();
    }

    async loadMGAs() {
        const response = await fetch('https://localhost:44363/api/mga', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        const data = await response.json();
        this.setState({ mgas: data, loading: false });
    }

    async createContract(entity1, entity2) {
        await fetch('https://localhost:44363/api/contract/establish/' + entity1 + '/' + entity2, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        this.loadMGAs();
    }

    async deleteContract(entity1, entity2) {
        await fetch('https://localhost:44363/api/contract/terminate/' + entity1 + '/' + entity2, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });
        this.loadMGAs();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <Business
                businesses={this.state.mgas}
                editBusiness={this.editMGA}
                deleteBusiness={this.deleteMGA}
                createContract={this.createContract}
                deleteContract={this.deleteContract}
            />;

        return (
            <div>
                <Jumbotron fluid>
                    <Container fluid>
                        <h1 className="display-3">MGA</h1>
                        <p className="lead">A managing general agent (MGA) or a managing general underwriter (MGU) is a specialized type of insurance agent or broker that has been granted underwriting authority by an insurer, according to the International Risk Management Institute (IRMI), and can administer programs and negotiate contracts for an insurer.</p>
                    </Container>
                </Jumbotron>
                {contents}
            </div>
        );
    }
}

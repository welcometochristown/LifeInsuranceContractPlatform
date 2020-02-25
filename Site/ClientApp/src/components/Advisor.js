import React, { Component } from 'react';
import { Person } from './Person.js'
import { Jumbotron, Container  } from 'reactstrap';

export class Advisor extends Component {
    static displayName = Advisor.name;

    constructor(props) {
        super(props);
        this.state = { advisors: [], loading: true };
    }

    componentDidMount() {
        this.loadAdvisors();
    }

    async createAdvisor(advisor) {

        await fetch('https://localhost:44363/api/advisor/', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(advisor)
        });
        this.loadAdvisors();
    }

    async editAdvisor(advisor) {

        await fetch('https://localhost:44363/api/advisor/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(advisor)
        });
        this.loadAdvisors();
    }

    async deleteAdvisor(advisor) {
        await fetch('https://localhost:44363/api/advisor/' + advisor.id, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });
        this.loadAdvisors();
    }

    async loadAdvisors() {
        const response = await fetch('https://localhost:44363/api/advisor', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        const data = await response.json();
        this.setState({ advisors: data, loading: false });
    }

    async createContract(entity1, entity2) {
        await fetch('https://localhost:44363/api/contract/establish/' + entity1 + '/' + entity2, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        this.loadAdvisors();
    }

    async deleteContract(entity1, entity2) {
        await fetch('https://localhost:44363/api/contract/terminate/' + entity1 + '/' + entity2, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });
        this.loadAdvisors();
    }

    render() {

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <Person
                people={this.state.advisors}
                createPerson={this.createAdvisor.bind(this)}
                editPerson={this.editAdvisor.bind(this)}
                deletePerson={this.deleteAdvisor.bind(this)}
                createContract={this.createContract.bind(this)}
                deleteContract={this.deleteContract.bind(this)}
                type="Advisor"
            />;

        return (
            <div>
                <Jumbotron fluid>
                    <Container fluid>
                        <h1 className="display-3">Advisors</h1>
                        <p className="lead">An insurance advisor can review your information (along with any existing policies) and give you an analysis of what exactly it is that you should have to sufficiently provide for your family. An advisor can also personally assist your loved ones in the event of a claim.</p>
                    </Container>
                </Jumbotron>
                {contents}
            </div>
        );
    }
}

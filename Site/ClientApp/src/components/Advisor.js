﻿import React, { Component } from 'react';
import { Person } from './Person.js'
import { Jumbotron, Container  } from 'reactstrap';

export class Advisor extends Component {
    static displayName = Advisor.name;

    constructor(props) {
        super(props);
        this.state = { advisors: [], loading: true };

        this.loadAdvisors = this.loadAdvisors.bind(this);

        this.editAdvisor = this.editAdvisor.bind(this);
        this.deleteAdvisor = this.deleteAdvisor.bind(this);

        this.createContract = this.createContract.bind(this);
        this.deleteContract = this.deleteContract.bind(this);
    }

    componentDidMount() {
        this.loadAdvisors();
    }

    async editAdvisor(advisor) {

        const response = await fetch('https://localhost:44363/api/advisor/', {
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
                editPerson={this.editAdvisor}
                deletePerson={this.deleteAdvisor}
                createContract={this.createContract}
                deleteContract={this.deleteContract}
                type={Advisor.displayName}
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

import React, { Component } from 'react';
import EntityDropdown from './EntityDropdown.js'
import { NavLink } from 'reactstrap';
import { ContractListModal, NewContractModal } from './Contract.js'
import { Button, Modal, ModalHeader, ModalBody } from 'reactstrap';
import { Input, Form, FormGroup, Label } from 'reactstrap';

class PersonEditModal extends Component {
    static displayName = PersonEditModal.name;

    constructor(props) {
        super(props);

        this.state = {
            isOpen: false,
            person: {
                name: ''
            },
            onSubmit: props.onSubmit           
        };
        this.closeModal = this.closeModal.bind(this);
        this.changeHandler = this.changeHandler.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    showModal(p) {
        this.setState({
            person: p,
            isOpen: true
        });
    }

    closeModal() {
        this.setState({ isOpen: false });
    }

    onSubmit(e) {
        e.preventDefault();

        this.setState({ isOpen: false });
        this.state.onSubmit(this.state.person);
    }

    changeHandler(event) {
        const name = event.target.name;
        const value = event.target.value;

        let newPerson = { ...this.state.person };
        newPerson[name] = value;
        this.setState({ person : newPerson });
    }

    modal(isOpen, person) {
        return (<Modal isOpen={isOpen}>
            <ModalHeader>{person.name}</ModalHeader>
            <ModalBody>
                <Form onSubmit={this.onSubmit}>

                    <FormGroup>
                        <Label for="firstName">First Name</Label>
                        <Input type="text" name="firstName" value={person.firstName} onChange={this.changeHandler} />
                    </FormGroup>
                    <FormGroup>
                        <Label for="lastName">Last Name</Label>
                        <Input type="text" name="lastName" value={person.lastName} onChange={this.changeHandler} />
                    </FormGroup>
                    <FormGroup>
                        <Label for="healthStatus">Health Status</Label>
                        <Input type="text" name="healthStatus" value={person.healthStatus} onChange={this.changeHandler} />
                    </FormGroup>
                    <FormGroup>
                        <Label for="address">Address</Label>
                        <Input type="text" name="address" value={person.address} onChange={this.changeHandler} />
                    </FormGroup>
                    <FormGroup>
                        <Label for="phoneNumber">Phone Number</Label>
                        <Input type="text" name="phoneNumber" value={person.phoneNumber} onChange={this.changeHandler} />
                    </FormGroup>

                    <div>
                        <Button type="submit" color="primary">Save</Button>{' '}
                        <Button color="secondary" onClick={this.closeModal}>Cancel</Button>{' '}
                    </div>

                </Form>
            </ModalBody>
        </Modal>);
    }

    render() {
        return (
            <div>{this.modal(this.state.isOpen, this.state.person)}</div>
        );
    }
}

class Person extends Component {
    static displayName = Person.name;

    constructor(props) {
        super(props);

        this.state = {
            people: props.people,

            editPerson: props.editPerson,
            deletePerson: props.deletePerson,

            createContract: props.createContract,
            deleteContract: props.deleteContract,

            entities: null,
            type : props.type
        }

        this.contractListElement = React.createRef();
        this.personEditElement = React.createRef();
        this.newContractElement = React.createRef();
    }

    componentDidMount() {
        this.loadEntities();
    }

    async loadEntities() {
        const response = await fetch('https://localhost:44363/api/entities/', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const data = await response.json();
        let arr = [];

        for (const property in data) {
            arr = arr.concat(data[property].map(function (v) {
                return {
                    id: v.id,
                    name: v.name
                }
            }));
        }

        this.setState({ entities: arr });
    }

    onViewContracts(person) {
        this.contractListElement.current.showModal(person);
    }

    onEditPerson(person) {
        this.personEditElement.current.showModal(person);
    }

    onDeletePerson(person) {
        this.state.deletePerson(person);
    }

    onCreateContract(person) {
        this.newContractElement.current.showModal(person, this.state.entities.filter((e) => e.id !== person.id));
    }

    onEditPersonSubmit(person) {
       this.state.editPerson(person);
    }

    onCreateContractSubmit(person, id) {
        this.state.createContract(person.id, id);
    }

    onTerminateContract(contract) {
        this.state.deleteContract(contract.entity1.id, contract.entity2.id);
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            people: nextProps.people,          
        });
    }

    renderPersonTable(people) {

        return (
            <div>
               
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>First Name</th>
                            <th>Last Name</th>
                            <th>Health Status</th>
                            <th>Address</th>
                            <th>Phone Number</th>
                            <th>Contracts</th>
                            <th/>
                        </tr>
                    </thead>
                    <tbody>
                      
                        {people.map(person =>
                            <tr key={person.id}>
                                <td>{person.firstName}</td>
                                <td>{person.lastName}</td>
                                <td>{person.healthStatus}</td>
                                <td>{person.address}</td>
                                <td>{person.phoneNumber}</td>
                                <td>
                                    <Button color="link" key={person.id} href="#" onClick={this.onViewContracts.bind(this, person)}>{person.contracts.length} Contract(s)</Button>
                                </td>
                                <td>
                                    <EntityDropdown
                                        key={person.id}
                                        edit={this.onEditPerson.bind(this, person)}
                                        delete={this.onDeletePerson.bind(this, person)}
                                        create={this.onCreateContract.bind(this, person)}
                                    />
                                </td>
                                
                            </tr>

                            )}
                        
                    </tbody>

             
                </table>
               

                <PersonEditModal ref={this.personEditElement} onSubmit={this.onEditPersonSubmit.bind(this)} />
                <NewContractModal ref={this.newContractElement} onSubmit={this.onCreateContractSubmit.bind(this)}/>
                <ContractListModal ref={this.contractListElement} onTerminate={this.onTerminateContract.bind(this)} />

            </div>
        );
    }


    render() {

        let contents = this.renderPersonTable(this.state.people);

        return (
            <div>
                {contents}
            </div>
        );
    }
}

export {
    Person,
    PersonEditModal
}
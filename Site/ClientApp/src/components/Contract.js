import React, { Component } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, ListGroup, ListGroupItem } from 'reactstrap';
import { Input, Form, FormGroup, Label } from 'reactstrap';

class NewContractModal extends Component {
    static displayName = NewContractModal.name;

    constructor(props) {
        super(props);
        this.state = {
            entity: {},
            entities: [],
            isOpen: false,
            selected: null,
            onSubmit: props.onSubmit
        };
        
        this.closeModal = this.closeModal.bind(this);
        this.changeHandler = this.changeHandler.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    showModal(e, entities) {
        let idSelected = null;
        if (entities && entities.length > 0) {
            idSelected = entities[0].id;
        }
        this.setState({
            entity: e,
            entities: entities,
            isOpen: true,
            selected: idSelected
        });
    }

    closeModal() {
        this.setState({ isOpen: false });
    }

    onSubmit(e) {
        e.preventDefault();

        this.setState({ isOpen: false });
        this.state.onSubmit(this.state.entity, this.state.selected);
    }

    changeHandler(event) {
        const value = event.target.value;
        this.setState({ selected: value });
    }

    modal(isOpen, entity, entities) {

        return (<Modal isOpen={isOpen} >
            <ModalHeader>Create Contract For {entity.name}</ModalHeader>
            <ModalBody>
                <Form onSubmit={this.onSubmit}>
                    <FormGroup>
                        <Label for="entitySelect">Select</Label>
                        <Input type="select" name="entitySelect" id="entitySelect" onChange={this.changeHandler}>
                            {entities.map(entity =>
                                <option key={entity.id} value={entity.id}>{entity.name}</option>)}
                        </Input>
                    </FormGroup>
                    <Button color="primary" type="submit" onClick={this.onSubmit}>Create</Button>
                    <Button color="secondary" onClick={this.closeModal}>Close</Button>
                </Form>
            </ModalBody>
        </Modal>);
    }

    render() {
        return (
            <div>{this.modal(this.state.isOpen, this.state.entity, this.state.entities)}</div>
        );
    }
}

class ContractListModal extends Component {
    static displayName = ContractListModal.name;

    constructor(props) {
        super(props);
        this.state = {
            entity: {
                name: '',
                contracts: []
            },
            isOpen: false,
            onTerminate : props.onTerminate
        };
        this.closeModal = this.closeModal.bind(this);
    }

    showModal(e) {
        this.setState({
            entity: e,
            isOpen: true
        });
    }

    terminateContract(contract) {
        this.state.onTerminate(contract);
        this.closeModal();
    }

    closeModal() {
        this.setState({ isOpen: false });
    }

    modal(isOpen, entity) {

        return (<Modal isOpen={isOpen} >
            <ModalHeader>{entity.name} Contracts</ModalHeader>
            <ModalBody>
                <ListGroup>
                {entity.contracts.map(contract =>
                    <ListGroupItem key={contract.entity1.id + '-' + contract.entity2.id}>
                        <div>
                            <Button color="danger" onClick={this.terminateContract.bind(this, contract)}>Terminate</Button>{'   '}
                            {contract.entity1.name === entity.name ? contract.entity2.name : contract.entity1.name}
                        </div>    
                    </ListGroupItem>
                   
                )}
                </ListGroup>
            </ModalBody>
            <ModalFooter>
                <Button color="secondary" onClick={this.closeModal}>Close</Button>
            </ModalFooter>
        </Modal>);
    }

    render() {
        return (
            <div>{this.modal(this.state.isOpen, this.state.entity)}</div> 
        );
    }
}

class Contract extends Component {
    static displayName = Contract.name;

    constructor(props) {
        super(props);
        this.state = { contracts: [], loading: true };
    }

    componentDidMount() {
        this.loadContracts();
    }

    async loadContracts() {
        const response = await fetch('https://localhost:44363/api/contract', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        });
        const data = await response.json();
        this.setState({ contracts: data, loading: false });
    }


    static renderContractTable(contracts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Entity Name</th>
                        <th>Entity Name</th>
                    </tr>
                </thead>
                <tbody>
                    {contracts.map(contract =>
                        <tr key={contract.entity1.id +'-' + contract.entity2.id}>
                            <td>{contract.entity1.name}</td>
                            <td>{contract.entity2.name}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Contract.renderContractTable(this.state.contracts);

        return (
            <div>
                <h1 id="tabelLabel" >Contracts</h1>
                {contents}
            </div>
        );
    }
}

export {
    Contract,
    ContractListModal,
    NewContractModal
}
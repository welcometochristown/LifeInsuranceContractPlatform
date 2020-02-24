import React, { Component } from 'react';
import EntityDropdown from './EntityDropdown.js'
import { NavLink } from 'reactstrap';
import { ContractListModal, NewContractModal } from './Contract.js'
import { Button, Modal, ModalHeader, ModalBody } from 'reactstrap';
import { Input, Form, FormGroup, Label } from 'reactstrap';

class BusinessEditModal extends Component {
    static displayName = BusinessEditModal.name;

    constructor(props) {
        super(props);

        this.state = {
            isOpen: false,
            business: {
                name: ''
            },
            onSubmit: props.onSubmit
        };
        this.closeModal = this.closeModal.bind(this);
        this.changeHandler = this.changeHandler.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    showModal(b) {
        this.setState({
            business: b,
            isOpen: true
        });
    }

    closeModal() {
        this.setState({ isOpen: false });
    }

    onSubmit(e) {
        e.preventDefault();

        this.setState({ isOpen: false });
        this.state.onSubmit(this.state.business);
    }

    changeHandler(event) {
        const name = event.target.name;
        const value = event.target.value;

        let newBusiness = { ...this.state.business };
        newBusiness[name] = value;
        this.setState({ business: newBusiness });
    }

    modal(isOpen, business) {
        return (<Modal isOpen={isOpen}>
            <ModalHeader>{business.name}</ModalHeader>
            <ModalBody>
                <Form onSubmit={this.onSubmit}>

                    <FormGroup>
                        <Label for="businessName">Business Name</Label>
                        <Input type="text" name="businessName" value={business.businessName} onChange={this.changeHandler} />
                    </FormGroup>
                    <FormGroup>
                        <Label for="businessAddress">Business Address</Label>
                        <Input type="text" name="businessAddress" value={business.businessAddress} onChange={this.changeHandler} />
                    </FormGroup>
                    <FormGroup>
                        <Label for="businessPhoneNumber">Business Phone Number</Label>
                        <Input type="text" name="businessPhoneNumber" value={business.businessPhoneNumber} onChange={this.changeHandler} />
                    </FormGroup>
                   
                    <Button type="submit" color="primary">Save</Button>
                    <Button color="secondary" onClick={this.closeModal}>Cancel</Button>

                </Form>
            </ModalBody>
        </Modal>);
    }

    render() {
        return (
            <div>{this.modal(this.state.isOpen, this.state.business)}</div>
        );
    }
}

class Business extends Component {
    static displayName = Business.name;

    constructor(props) {
        super(props);

        this.state = {
            businesses: props.businesses,

            editBusiness: props.editBusiness,
            deleteBusiness: props.deleteBusiness,

            createContract: props.createContract,
            deleteContract: props.deleteContract
        }

        this.contractListElement = React.createRef();
        this.businessEditElement = React.createRef();
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

    onViewContracts(business) {
        this.contractListElement.current.showModal(business);
    }

    onEditBusiness(business) {
        this.businessEditElement.current.showModal(business);
    }

    onDeleteBusiness(business) {
        this.state.deleteBusiness(business);
    }

    onCreateContract(business) {
        this.newContractElement.current.showModal(business, this.state.entities.filter((e) => e.id !== business.id));
    }

    onEditBusinessSubmit(business) {
        this.state.editBusiness(business);
    }

    onCreateContractSubmit(business, id) {
        this.state.createContract(business.id, id);
    }

    onTerminateContract(contract) {
        this.state.deleteContract(contract.entity1.id, contract.entity2.id);
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            business: nextProps.business,
        });
    }

    renderBusinessTable(businesses) {
        return (
            <div>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Business Name</th>
                            <th>Business Address</th>
                            <th>Business Phone Number</th>
                            <th>Contracts</th>
                            <th />
                        </tr>
                    </thead>
                    <tbody>
                        {businesses.map(business =>
                            <tr key={business.id}>
                                <td>{business.businessName}</td>
                                <td>{business.businessAddress}</td>
                                <td>{business.businessPhoneNumber}</td>
                                <td>
                                    <NavLink className="nav-link-blue" key={business.id} href="#" onClick={this.onViewContracts.bind(this, business)}>{business.contracts.length} Contract(s)</NavLink>
                                </td>
                                <td>
                                    <EntityDropdown key={business.id} edit={this.onEditBusiness.bind(this, business)} delete={this.onDeleteBusiness.bind(this, business)} create={this.onCreateContract.bind(this, business)} />
                                </td>

                            </tr>

                        )}
                    </tbody>
                </table>

                <BusinessEditModal ref={this.businessEditElement} onSubmit={this.onEditBusinessSubmit.bind(this)} />
                <NewContractModal ref={this.newContractElement} onSubmit={this.onCreateContractSubmit.bind(this)} />
                <ContractListModal ref={this.contractListElement} onTerminate={this.onTerminateContract.bind(this)} />
               
            </div>
        );
    }


    render() {

        let contents = this.renderBusinessTable(this.state.businesses);

        return (
            <div>
                {contents}
            </div>
        );
    }
}

export {
    Business,
    BusinessEditModal
}
import React, { useState } from 'react';
import { ButtonDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

function EntityDropdown(props) {

    const [dropdownOpen, setOpen] = useState(false);
    const toggle = () => setOpen(!dropdownOpen);

    return (
        <ButtonDropdown isOpen={dropdownOpen} toggle={toggle}>
            <DropdownToggle color='primary' caret>
                Actions
           </DropdownToggle>
            <DropdownMenu>
                <DropdownItem onClick={props.edit}>Edit</DropdownItem>
                <DropdownItem onClick={props.delete}>Delete</DropdownItem>
                <DropdownItem divider />
                <DropdownItem onClick={props.create}>Create Contract</DropdownItem>
            </DropdownMenu>
        </ButtonDropdown>
    );
}

export default EntityDropdown;
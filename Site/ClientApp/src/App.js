import React, { Component } from 'react';
import { Route } from 'react-router';

import { Home } from './components/Home';
import { Layout } from './components/Layout';
import { Advisor } from './components/Advisor';
import { Carrier } from './components/Carrier';
import { MGA } from './components/MGA';
import { Contract } from './components/Contract';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/advisor' component={Advisor} />
            <Route path='/carrier' component={Carrier} />
            <Route path='/mga' component={MGA} />
            <Route path='/contract' component={Contract} />
      </Layout>
    );
  }
}

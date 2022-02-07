# Lab 1

Your task is to let the two tests in SeatAllocatorShould test pass on to the model we designed in the responsiblity mapping. You can do this by yourself, in a pair or even do a ensemble programming with multiple people.
Start implementing the tests using the ExternalDependencies 'AuditoriumSeatingAdapter' were you can 'GetAuditoriumSeating' to use 'AuditoriumSeating' as input state to your model for the examples modelled in the example mapping.

You can find the states visualised in '../../AuditoriumLayoutExamples.md', use the corresponding ID to call the state.

We won't be doing puristic TDD, as in we will use the domain model designed from the responsbility mapping to create objects which are the expression of our ubiqituous language in code. You can just create them in the TheatherSuggestions.Tests project and focus on passing the tests on our domain model. We deal with project structure in a later lab.
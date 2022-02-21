# Lab 1

Your task is to let the two tests in SeatAllocatorShould test pass on to the model we designed in the responsiblity mapping. You can do this by yourself, in a pair or even do a ensemble programming with multiple people.
Start implementing the tests using the ExternalDependencies 'AuditoriumSeatingAdapter' were you can 'GetAuditoriumSeating' to use 'AuditoriumSeating' as input state to your model for the examples modelled in the example mapping.

You can find the states visualised in '../../AuditoriumLayoutExamples.md', use the corresponding ID to call the state.

We won't be doing puristic TDD, as in we will use the domain model designed from the responsbility mapping to create objects which are the expression of our ubiqituous language in code. You can just create them in the TheatherSuggestions.Tests project and focus on passing the tests on our domain model. We deal with project structure in a later lab.

# Lab 2

We extracted our domain model to the 'SeatsSuggestions' project, and refactored our stub/fake mechanism int he Tests. Now it is time to implement the next test in 'SeatAllocatorShould' called 'Offer_several_suggestions_ie_1_per_PricingCategory_and_other_one_without_category_affinity' which fails. You need to find out why it failed, that is why we gave you a couple of unit tests to work with and find the bug!

# Lab 3 Part 1 - Offer seats nearer to the middle of a row

Now that we used the tactical patterns to make our design more supple to changes of our business, we can start implementing more complex domain logic.
In lab 3, part 1 we added a new failing Acceptance Test that we modeled together in Example Mapping about offering seats nearer to the middle of a row.
We also added two new unit test to the 'RowShould' that fails that will help you implement the test. You can first have a discussion how you would design this more complex domain logic.
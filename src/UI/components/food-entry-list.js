import Paging from './paging';
import { foodEntryService } from '../services/food-entry-service';
import { alertService } from '../services/alert-service';


export default function FoodEntryList({ page, pageSize, totalCount, foodEntries, onPageChange, isAdmin, showModal }) {
    if (!foodEntries || foodEntries.length === 0) {
        return <></>;
    }

    const deleteFoodEntry = async (foodEntry) => {
        const willDelete = await alertService.confirm("Are you sure?", `Do you want to delete the entry with the food ${foodEntry.foodName} of ${foodEntry.user.name}?`);

        if (!willDelete)
            return;

        var result = await foodEntryService.deleteFoodEntry(foodEntry.user.id, foodEntry.id).catch(e => {
            alertService.error(e.toString());
            return false;
        });

        if (result) {
            alertService.success("Deleted!", "The entry is deleted.");
            onPageChange(page);
        }
    }

    return (
        <div className="w-full mb-8 overflow-hidden rounded-lg shadow-xs">
            <div className="w-full overflow-x-auto">
                <table className="w-full whitespace-no-wrap">
                    <thead>
                        <tr className="text-xs font-semibold tracking-wide text-gray-500 uppercase border-b bg-gray-50">
                            {isAdmin && <th className="px-4 py-3 text-left">User</th>}
                            <th className="px-4 py-3 text-left">Food</th>
                            <th className="px-4 py-3 text-left">Calorie</th>
                            <th className="px-4 py-3 text-left">Date</th>
                            {isAdmin && <th className="px-4 py-3">Actions</th>}
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y">
                        {foodEntries.map(f =>
                            <tr className="text-gray-700" key={f.id}>
                                {isAdmin && <td className="px-4 py-3">
                                    <div className="flex items-center text-sm">
                                        <div>
                                            <p className="font-semibold">
                                                {f.user.name}</p>
                                            {<p className="text-xs text-gray-600">
                                                {f.user.email}
                                            </p>}
                                        </div>
                                    </div>
                                </td>}
                                <td className="px-4 py-3 text-sm">
                                    <span>{f.foodName}</span>
                                </td>
                                <td className="px-4 py-3 text-sm">
                                    {f.calorie === 0 && <span className="text-green-600 material-icons material-icons-outlined">sentiment_very_satisfied</span>}
                                    {f.calorie !== 0 && <span>{f.calorie}</span>}
                                </td>
                                <td className="px-4 py-3 text-sm">
                                    {new Date(f.date).toLocaleDateString()}
                                </td>
                                {isAdmin && <td className="px-4 py-3">
                                    <div className="flex justify-end space-x-4 text-sm">
                                        <button
                                            className="flex items-center justify-between px-2 py-2 text-sm font-medium leading-5 text-purple-600 rounded-lg focus:outline-none focus:shadow-outline-gray"
                                            aria-label="Edit"
                                            onClick={() => showModal(f)}>
                                            <svg
                                                className="w-5 h-5"
                                                aria-hidden="true"
                                                fill="currentColor"
                                                viewBox="0 0 20 20">
                                                <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"></path>
                                            </svg>
                                        </button>
                                        <button
                                            className="flex items-center justify-between px-2 py-2 text-sm font-medium leading-5 text-purple-600 rounded-lg focus:outline-none focus:shadow-outline-gray"
                                            aria-label="Delete"
                                            onClick={() => deleteFoodEntry(f)}>
                                            <svg
                                                className="w-5 h-5"
                                                aria-hidden="true"
                                                fill="currentColor"
                                                viewBox="0 0 20 20">
                                                <path
                                                    fillRule="evenodd"
                                                    d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
                                                    clipRule="evenodd"></path>
                                            </svg>
                                        </button>
                                    </div>
                                </td>}
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
            <Paging page={page} pageSize={pageSize} count={foodEntries.length} totalCount={totalCount} onPageChange={onPageChange} />
        </div>
    )
}
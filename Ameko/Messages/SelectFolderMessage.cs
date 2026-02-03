// SPDX-License-Identifier: GPL-3.0-only

namespace Ameko.Messages;

public class SelectFolderMessage(int id)
{
    public int Id { get; } = id;
}

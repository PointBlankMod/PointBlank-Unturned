using System.Xml;
using System.Collections.Generic;
using System.Linq;
using PointBlank.API.Permissions;

namespace PointBlank.API.Steam
{
    /// <summary>
    /// The steam group instance
    /// </summary>
    public class SteamGroup : IPermitable
    {
        #region Variables
        private List<PointBlankPermission> _permissions = new List<PointBlankPermission>();
        private List<SteamGroup> _inherits = new List<SteamGroup>();

        private List<string> _prefixes = new List<string>();
        private List<string> _suffixes = new List<string>();
        #endregion

        #region Properties
        /// <summary>
        /// The steam group name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The steam group ID
        /// </summary>
        public ulong Id { get; private set; }
        /// <summary>
        /// The amount of members in the group
        /// </summary>
        public int MemberCount { get; private set; }
        /// <summary>
        /// The amount of members currently online
        /// </summary>
        public int MembersOnline { get; private set; }
        /// <summary>
        /// The amount of members in game
        /// </summary>
        public int MembersInGame { get; private set; }
        /// <summary>
        /// The amount of members in chat
        /// </summary>
        public int MembersInChat { get; private set; }

        /// <summary>
        /// The group permissions
        /// </summary>
        public PointBlankPermission[] Permissions => _permissions.ToArray();
        /// <summary>
        /// The group inherits
        /// </summary>
        public SteamGroup[] Inherits => _inherits.ToArray();

        /// <summary>
        /// The group prefixes
        /// </summary>
        public string[] Prefixes => _prefixes.ToArray();
        /// <summary>
        /// The group suffixes
        /// </summary>
        public string[] Suffixes => _suffixes.ToArray();

        /// <summary>
        /// The group cooldown for commands
        /// </summary>
        public int Cooldown { get; set; }

        /// <summary>
        /// Should the group be ignored while saving
        /// </summary>
        public bool Ignore { get; private set; }
        #endregion

        /// <summary>
        /// The steam group instance using async
        /// </summary>
        /// <param name="id">The ID of the steam group</param>
        /// <param name="cooldown">The default cooldown for the steam group</param>
        /// <param name="downloadData">Should the information for the steam group be downloaded</param>
        /// <param name="ignore">Should the group be ignored while saving</param>
        public SteamGroup(ulong id, int cooldown = -1, bool downloadData = false, bool ignore = true)
        {
            // Set the variables
            this.Id = id;
            this.Cooldown = cooldown;
            this.Ignore = ignore;

            // Run the code
            if (downloadData)
                DownloadData();
        }
        /// <summary>
        /// The steam group instance using async
        /// </summary>
        /// <param name="id">The ID of the steam group</param>
        /// <param name="downloadData">Should the information for the steam group be downloaded</param>
        /// <param name="ignore">Should the group be ignored while saving</param>
        public SteamGroup(ulong id, bool downloadData = false, bool ignore = true)
        {
            // Set the variables
            this.Id = id;
            this.Ignore = ignore;

            // Run the code
            if (downloadData)
                DownloadData();
        }

        #region Public Functions
        /// <summary>
        /// Downloads the steam group data from steam
        /// </summary>
        public void DownloadData()
        {
            XmlDocument document = new XmlDocument();
            document.Load($"http://steamcommunity.com/gid/{Id.ToString()}/memberslistxml/?xml=1");
            XmlNode root = document.DocumentElement;

            // Set the data
            if (root != null)
            {
                Name = root.SelectSingleNode("groupDetails/groupName").InnerText.Replace("<![CDATA[ ", "").Replace(" ]]>", "");
                MemberCount = int.Parse(root.SelectSingleNode("groupDetails/memberCount").InnerText);
                MembersOnline = int.Parse(root.SelectSingleNode("groupDetails/membersOnline").InnerText);
                MembersInGame = int.Parse(root.SelectSingleNode("groupDetails/membersInGame").InnerText);
                MembersInChat = int.Parse(root.SelectSingleNode("groupDetails/membersInChat").InnerText);
            }
            else
            {
                Name = "";
                MemberCount = 0;
                MembersOnline = 0;
                MembersInGame = 0;
                MembersInChat = 0;
            }
        }

        /// <summary>
        /// Add a permission to the steam group
        /// </summary>
        /// <param name="permission">The permission to add</param>
        public void AddPermission(string permission)
        {
            PointBlankPermission perm = PointBlankPermissionManager.GetPermission(this, permission);

            if (perm == null)
                return;
            AddPermission(perm);
        }
        /// <summary>
        /// Add a permission to the steam group
        /// </summary>
        /// <param name="permission">The permission to add</param>
        public void AddPermission(PointBlankPermission permission)
        {
            if (_permissions.Contains(permission))
                return;

            _permissions.Add(permission);
            SteamGroupEvents.RunPermissionAdded(this, permission);
        }

        /// <summary>
        /// Remove a permission from the steam group
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public void RemovePermission(string permission)
        {
            PointBlankPermission perm = PointBlankPermissionManager.GetPermission(this, permission);

            if (perm == null)
                return;
            RemovePermission(perm);
        }
        /// <summary>
        /// Remove a permission from the steam group
        /// </summary>
        /// <param name="permission">The permission to remove</param>
        public void RemovePermission(PointBlankPermission permission)
        {
            if (!_permissions.Contains(permission))
                return;

            _permissions.Remove(permission);
            SteamGroupEvents.RunPermissionRemoved(this, permission);
        }

        /// <summary>
        /// Add a prefix to the steam group
        /// </summary>
        /// <param name="prefix">The prefix to add</param>
        public void AddPrefix(string prefix)
        {
            if (_prefixes.Contains(prefix))
                return;

            _prefixes.Add(prefix);
            SteamGroupEvents.RunPrefixAdded(this, prefix);
        }

        /// <summary>
        /// Remove a prefix from the steam group
        /// </summary>
        /// <param name="prefix">The prefix to remove</param>
        public void RemovePrefix(string prefix)
        {
            if (!_prefixes.Contains(prefix))
                return;

            _prefixes.Remove(prefix);
            SteamGroupEvents.RunPrefixRemoved(this, prefix);
        }

        /// <summary>
        /// Adds a suffix to the steam group
        /// </summary>
        /// <param name="suffix">The suffix to add</param>
        public void AddSuffix(string suffix)
        {
            if (_suffixes.Contains(suffix))
                return;

            _suffixes.Add(suffix);
            SteamGroupEvents.RunSuffixAdded(this, suffix);
        }

        /// <summary>
        /// Removes a suffix from the steam group
        /// </summary>
        /// <param name="suffix">The suffix to remove</param>
        public void RemoveSuffix(string suffix)
        {
            if (!_suffixes.Contains(suffix))
                return;

            _suffixes.Remove(suffix);
            SteamGroupEvents.RunSuffixRemoved(this, suffix);
        }

        /// <summary>
        /// Adds an inherit to the steam group
        /// </summary>
        /// <param name="group">The group to add to inherits</param>
        public void AddInherit(SteamGroup group)
        {
            if (_inherits.Contains(group))
                return;
            if (_inherits.Count(a => a.Id == group.Id) > 0)
                return;

            _inherits.Add(group);
            SteamGroupEvents.RunInheritAdded(this, group);
        }

        /// <summary>
        /// Removes an inherit from the steam group
        /// </summary>
        /// <param name="group">The inherit to remove</param>
        public void RemoveInherit(SteamGroup group)
        {
            if (!_inherits.Contains(group))
                return;

            _inherits.Remove(group);
            SteamGroupEvents.RunInheritRemoved(this, group);
        }

        /// <summary>
        /// Gets the list of all permissions including inheritences
        /// </summary>
        /// <returns>The list of all permissions including inheritences</returns>
        public PointBlankPermission[] GetPermissions()
        {
            List<PointBlankPermission> permissions = new List<PointBlankPermission>();

            permissions.AddRange(Permissions);
            PointBlankTools.ForeachLoop<SteamGroup>(Inherits, delegate (int index, SteamGroup value)
            {
                PointBlankTools.ForeachLoop<PointBlankPermission>(value.GetPermissions(), delegate (int i, PointBlankPermission v)
                {
                    if (permissions.Contains(v))
                        return;

                    permissions.Add(v);
                });
            });

            return permissions.ToArray();
        }

        /// <summary>
        /// Checks if the steam group has the permission specified
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>If the steam group has the permission specified</returns>
        public bool HasPermission(string permission)
        {
            PointBlankPermission perm = new PointBlankPermission(permission);

            if (perm == null)
                return false;
            return HasPermission(perm);
        }
        /// <summary>
        /// Checks if the steam group has the permission specified
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>If the steam group has the permission specified</returns>
        public bool HasPermission(PointBlankPermission permission)
        {
            PointBlankPermission[] permissions = GetPermissions();

            for (int i = 0; i < permissions.Length; i++)
                if (permissions[i].IsOverlappingPermission(permission))
                    return true;
            return false;
        }

        /// <summary>
        /// Converts a string to a permission object or returns null if not found
        /// </summary>
        /// <param name="permission">The permission string used for the conversion</param>
        /// <returns>The permission object or null if not found</returns>
        public PointBlankPermission GetPermission(string permission) => GetPermissions().FirstOrDefault(a => a.Permission == permission);
        #endregion
    }
}
